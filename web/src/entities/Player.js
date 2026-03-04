import Phaser from 'phaser';
import { PlayerData } from '../data/PlayerData.js';
import { createFireball, createIcepick } from '../data/Skill.js';

export class Player extends Phaser.Physics.Arcade.Sprite {
  constructor(scene, x, y) {
    super(scene, x, y, 'player-idle1');
    scene.add.existing(this);
    scene.physics.add.existing(this);

    this.setScale(0.5);
    this.body.setSize(60, 120);
    this.body.setOffset(20, 10);
    this.setCollideWorldBounds(true);

    // Data
    this.data_ = new PlayerData(1);
    this.health = this.data_.maxHealth;
    this.energy = this.data_.maxEnergy;
    this.direction = 1;
    this.jumpCount = 0;
    this.canJump = true;
    this.jumpTimer = null;

    // Skills
    this.learnedSkills = [createFireball(), createIcepick()];
    this.selectedSkills = [this.learnedSkills[0], this.learnedSkills[1]];
    this.currentSkill = 0;
    this.skillCooling = [false, false, false, false];

    // Input
    this.keys = {
      W: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.W),
      A: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.A),
      D: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.D),
      SPACE: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.SPACE),
      ONE: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.ONE),
      TWO: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.TWO),
      THREE: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.THREE),
      FOUR: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.FOUR),
      ENTER: scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.ENTER),
    };

    this.locked = false;
    this.play('player-idle');
  }

  get maxHealth() { return this.data_.maxHealth; }
  get maxEnergy() { return this.data_.maxEnergy; }

  update(time, delta) {
    if (this.locked) return;

    const dt = delta / 1000;
    const onGround = this.body.blocked.down;

    // Regen
    this.health = Math.min(this.health + this.data_.healthRegen * dt, this.data_.maxHealth);
    this.energy = Math.min(this.energy + this.data_.energyRegen * dt, this.data_.maxEnergy);

    // Reset jump count on ground
    if (onGround) {
      this.jumpCount = 0;
    }

    // Skill selection
    if (Phaser.Input.Keyboard.JustDown(this.keys.ONE)) this.currentSkill = 0;
    if (Phaser.Input.Keyboard.JustDown(this.keys.TWO)) this.currentSkill = 1;
    if (Phaser.Input.Keyboard.JustDown(this.keys.THREE)) this.currentSkill = 2;
    if (Phaser.Input.Keyboard.JustDown(this.keys.FOUR)) this.currentSkill = 3;

    // Attack
    if (Phaser.Input.Keyboard.JustDown(this.keys.SPACE)) {
      this.tryAttack();
    }

    // Jump
    if (Phaser.Input.Keyboard.JustDown(this.keys.W)) {
      this.tryJump();
    }

    // Run
    let running = false;
    if (this.keys.D.isDown) {
      this.direction = 1;
      running = true;
      const accel = onGround ? this.data_.runAccel : this.data_.airRunAccel;
      this.body.velocity.x += accel * dt;
    } else if (this.keys.A.isDown) {
      this.direction = -1;
      running = true;
      const accel = onGround ? this.data_.runAccel : this.data_.airRunAccel;
      this.body.velocity.x -= accel * dt;
    }

    // Clamp horizontal speed
    const maxSpeed = this.data_.maxRunSpeed;
    this.body.velocity.x = Phaser.Math.Clamp(this.body.velocity.x, -maxSpeed, maxSpeed);

    // Apply friction when not running and on ground
    if (!running && onGround) {
      if (Math.abs(this.body.velocity.x) < 10) {
        this.body.velocity.x = 0;
      } else {
        this.body.velocity.x *= 0.85;
      }
    }

    // Flip sprite
    this.setFlipX(this.direction < 0);

    // Animations
    if (!onGround) {
      if (this.anims.currentAnim?.key !== 'player-jump') {
        this.play('player-jump', true);
      }
    } else if (running) {
      this.play('player-run', true);
      // Footstep sounds
      if (this.anims.currentFrame) {
        const frameIndex = this.anims.currentFrame.index;
        if (frameIndex === 2 || frameIndex === 5) {
          if (!this._lastStepFrame || this._lastStepFrame !== frameIndex) {
            this.scene.sound.play('sfx-step2', { volume: 0.3 });
            this._lastStepFrame = frameIndex;
          }
        } else {
          this._lastStepFrame = null;
        }
      }
    } else {
      this.play('player-idle', true);
    }

    // Pause
    if (Phaser.Input.Keyboard.JustDown(this.keys.ENTER)) {
      this.scene.togglePause();
    }

    // Respawn on death
    if (this.health <= 0) {
      this.health = this.data_.maxHealth;
      this.setPosition(100, 200);
      this.body.velocity.set(0, 0);
    }
  }

  tryJump() {
    if (this.canJump && this.jumpCount < this.data_.maxJump) {
      this.body.velocity.y = this.data_.jumpVelocity;
      this.jumpCount++;
      this.play('player-jump', true);
      this.scene.sound.play('sfx-jump', { volume: 0.5 });

      // Jump cooldown
      this.canJump = false;
      this.scene.time.delayedCall(this.data_.jumpWaitTime, () => {
        this.canJump = true;
      });
    }
  }

  tryAttack() {
    if (this.currentSkill >= this.selectedSkills.length) return;
    if (this.skillCooling[this.currentSkill]) return;
    if (this.energy < 10) return;

    this.energy -= 10;
    const skill = this.selectedSkills[this.currentSkill];

    this.skillCooling[this.currentSkill] = true;
    this.scene.time.delayedCall(skill.originDelay * 1000, () => {
      this.skillCooling[this.currentSkill] = false;
    });

    // Spawn projectile after a brief delay
    this.scene.spawnPlayerSkill(this, skill);
  }

  applyDamage(amount) {
    this.health -= amount;
    this.scene.sound.play('sfx-player-land', { volume: 0.7 });
  }

  collectPickup(skill) {
    // Check if we already know this skill
    const existing = this.learnedSkills.find(s => s.name === skill.name);
    if (existing) {
      existing.levelUp();
    } else {
      this.learnedSkills.push(skill);
      for (let i = 0; i < 4; i++) {
        if (!this.selectedSkills[i]) {
          this.selectedSkills[i] = skill;
          break;
        }
      }
    }
    this.energy = Math.min(this.energy + 10, this.data_.maxEnergy);
    this.health = Math.min(this.health + 5, this.data_.maxHealth);
    this.scene.sound.play('sfx-pickup', { volume: 1.0 });
  }
}
