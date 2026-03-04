import Phaser from 'phaser';
import { EnemyData } from '../data/EnemyData.js';
import { createEnemyIcepick } from '../data/Skill.js';

const STATE = {
  IDLE: 'IDLE',
  WANDER: 'WANDER',
  ATTACK: 'ATTACK',
  DYING: 'DYING',
  DEAD: 'DEAD',
};

export class Enemy extends Phaser.Physics.Arcade.Sprite {
  constructor(scene, x, y, pathNodes = []) {
    super(scene, x, y, 'player-idle1'); // reuse player sprites for enemy (tinted)
    scene.add.existing(this);
    scene.physics.add.existing(this);

    this.setScale(0.45);
    this.setTint(0x8888ff); // blue tint to differentiate from player
    this.body.setSize(60, 120);
    this.body.setOffset(20, 10);
    this.setCollideWorldBounds(true);

    // Data
    this.data_ = new EnemyData(1);
    this.health = this.data_.maxHealth;
    this.energy = this.data_.maxEnergy;
    this.direction = 1;

    // Skills
    this.selectedSkills = [createEnemyIcepick()];
    this.currentSkill = 0;
    this.skillCooling = false;

    // AI state
    this.state = STATE.IDLE;
    this.stateTimer = 0;

    // Path patrol
    this.pathNodes = pathNodes.length > 0 ? pathNodes : [{ x: x - 150, y }, { x: x + 150, y }];
    this.nextNode = 1;

    // Health bar
    this.healthBarBg = scene.add.rectangle(x, y - 40, 50, 6, 0x333333);
    this.healthBar = scene.add.rectangle(x, y - 40, 50, 6, 0xff0000);
    this.healthBarBg.setDepth(5);
    this.healthBar.setDepth(6);

    this.play('player-idle');
  }

  update(time, delta) {
    if (this.state === STATE.DEAD) return;

    const dt = delta / 1000;
    const player = this.scene.player;

    // Update health bar position
    this.healthBarBg.setPosition(this.x, this.y - 45);
    this.healthBar.setPosition(this.x, this.y - 45);
    const healthPct = Math.max(0, this.health / this.data_.maxHealth);
    this.healthBar.setSize(50 * healthPct, 6);

    // Check dying
    if (this.health <= 0 && this.state !== STATE.DYING) {
      this.enterDying();
      return;
    }

    // Check hostile (player in range)
    const isHostile = player && this.isPlayerInRange(player);

    switch (this.state) {
      case STATE.IDLE:
        this.body.velocity.x = 0;
        this.stateTimer -= dt;
        if (isHostile) {
          this.state = STATE.ATTACK;
        } else if (this.stateTimer <= 0) {
          this.state = STATE.WANDER;
          this.setWanderDirection();
        }
        break;

      case STATE.WANDER:
        if (isHostile) {
          this.state = STATE.ATTACK;
          this.body.velocity.x = 0;
          break;
        }

        this.body.velocity.x = this.direction * this.data_.maxWanderSpeed;
        this.setFlipX(this.direction < 0);
        this.play('player-run', true);

        // Check if reached next node
        const target = this.pathNodes[this.nextNode];
        if (Math.abs(this.x - target.x) < this.data_.minNodeDistance) {
          this.nextNode = (this.nextNode + 1) % this.pathNodes.length;
          this.state = STATE.IDLE;
          this.stateTimer = this.data_.nodeWaitTime / 1000;
          this.body.velocity.x = 0;
          this.play('player-idle', true);
        }
        break;

      case STATE.ATTACK:
        if (!isHostile) {
          this.state = STATE.WANDER;
          this.setWanderDirection();
          break;
        }

        // Face player
        this.direction = player.x > this.x ? 1 : -1;
        this.setFlipX(this.direction < 0);
        this.body.velocity.x = 0;
        this.play('player-idle', true);

        // Attack if cooldown ready
        if (!this.skillCooling) {
          this.skillCooling = true;
          const skill = this.selectedSkills[this.currentSkill];

          this.scene.spawnEnemySkill(this, skill);

          this.scene.time.delayedCall(skill.originDelay * 1000, () => {
            this.skillCooling = false;
          });
        }
        break;

      case STATE.DYING:
        this.body.velocity.x = 0;
        // Dying handled by timer in enterDying
        break;
    }
  }

  isPlayerInRange(player) {
    return Math.abs(player.x - this.x) <= this.data_.viewRangeX &&
           Math.abs(player.y - this.y) <= this.data_.viewRangeY;
  }

  setWanderDirection() {
    const target = this.pathNodes[this.nextNode];
    this.direction = target.x > this.x ? 1 : -1;
    this.setFlipX(this.direction < 0);
  }

  applyDamage(amount) {
    this.health -= amount;
    this.scene.sound.play('sfx-enemy-hit', { volume: 0.8 });
  }

  enterDying() {
    this.state = STATE.DYING;
    this.body.velocity.set(0, 0);
    this.setAlpha(0.5);
    this.scene.sound.play('sfx-enemy-die', { volume: 0.8 });

    // Spawn pickup
    this.scene.spawnPickup(this.x, this.y, this.selectedSkills[0]);

    this.scene.time.delayedCall(1000, () => {
      this.state = STATE.DEAD;
      this.healthBarBg.destroy();
      this.healthBar.destroy();
      this.destroy();
    });
  }
}
