import Phaser from 'phaser';
import { Player } from '../entities/Player.js';
import { Enemy } from '../entities/Enemy.js';
import { Fireball } from '../entities/Fireball.js';
import { Icepick } from '../entities/Icepick.js';
import { HUD } from '../ui/HUD.js';
import { DamageText } from '../ui/DamageText.js';
import { WORLD } from '../constants.js';

export class GameScene extends Phaser.Scene {
  constructor() {
    super('GameScene');
  }

  create() {
    this.isPaused = false;

    // World bounds
    this.physics.world.setBounds(0, 0, WORLD.width, WORLD.height);

    // Background
    this.add.rectangle(WORLD.width / 2, WORLD.height / 2, WORLD.width, WORLD.height, 0x1a1a3e);

    // Snowflakes
    this.snowflakes = [];
    for (let i = 0; i < 15; i++) {
      const sf = this.add.image(
        Phaser.Math.Between(0, WORLD.width),
        Phaser.Math.Between(-100, WORLD.height),
        'menu-snowflake'
      ).setScale(Phaser.Math.FloatBetween(0.05, 0.15)).setAlpha(0.4).setDepth(1);
      sf.speed = Phaser.Math.FloatBetween(0.3, 1.0);
      sf.sway = Phaser.Math.FloatBetween(0.5, 2.0);
      this.snowflakes.push(sf);
    }

    // Create terrain platforms
    this.platforms = this.physics.add.staticGroup();
    this.buildLevel();

    // Player
    this.player = new Player(this, 100, 600);

    // Enemies
    this.enemies = this.add.group();
    this.spawnEnemies();

    // Skill projectile groups
    this.playerSkills = this.add.group();
    this.enemySkills = this.add.group();

    // Pickups
    this.pickups = this.add.group();

    // Damage texts
    this.damageTexts = [];

    // Physics collisions
    this.physics.add.collider(this.player, this.platforms, this.onPlayerLand, null, this);
    this.physics.add.collider(this.enemies, this.platforms);

    // Skill overlaps
    this.physics.add.overlap(this.playerSkills, this.enemies, this.onPlayerSkillHitEnemy, null, this);
    this.physics.add.overlap(this.enemySkills, this.player, this.onEnemySkillHitPlayer, null, this);

    // Pickup overlap
    this.physics.add.overlap(this.player, this.pickups, this.onPlayerCollectPickup, null, this);

    // Camera
    this.cameras.main.setBounds(0, 0, WORLD.width, WORLD.height);
    this.cameras.main.startFollow(this.player, true, 0.1, 0.1);

    // HUD
    this.hud = new HUD(this);

    // Background music
    this.bgm = this.sound.add('bgm-ninja', { loop: true, volume: 0.2 });
    this.bgm.play();

    // Pause text (hidden)
    this.pauseText = this.add.text(640, 360, 'PAUSED', {
      fontSize: '48px', fill: '#ffffff', fontStyle: 'bold',
      stroke: '#000000', strokeThickness: 4,
    }).setOrigin(0.5).setScrollFactor(0).setDepth(200).setVisible(false);

    this.elapsedTime = 0;
  }

  buildLevel() {
    // Ground floor — long ground spanning the level
    this.addPlatform(0, 850, WORLD.width, 60, 'ice-ground');

    // Ground top layer
    this.addPlatform(0, 795, WORLD.width, 55, 'ice-ground-top');

    // Floating platforms
    const platformPositions = [
      { x: 300, y: 620 },
      { x: 550, y: 500 },
      { x: 800, y: 600 },
      { x: 1100, y: 480 },
      { x: 1400, y: 550 },
      { x: 1700, y: 420 },
      { x: 2000, y: 600 },
      { x: 2300, y: 500 },
      { x: 2600, y: 650 },
      { x: 2900, y: 450 },
      { x: 3200, y: 550 },
      { x: 3500, y: 400 },
      { x: 3800, y: 600 },
      { x: 4100, y: 500 },
      { x: 4400, y: 650 },
      { x: 4700, y: 450 },
      { x: 5000, y: 550 },
      { x: 5300, y: 400 },
      { x: 5600, y: 600 },
    ];

    for (const pos of platformPositions) {
      this.addPlatform(pos.x, pos.y, 160, 30, 'ice-platform');
    }
  }

  addPlatform(x, y, width, height, texture) {
    // Use tiled images for wider platforms, single image for smaller
    const platform = this.platforms.create(x + width / 2, y + height / 2, texture);
    platform.setDisplaySize(width, height);
    platform.refreshBody();
    return platform;
  }

  spawnEnemies() {
    const enemyConfigs = [
      { x: 600, y: 700, path: [{ x: 400, y: 700 }, { x: 800, y: 700 }] },
      { x: 1200, y: 700, path: [{ x: 1000, y: 700 }, { x: 1400, y: 700 }] },
      { x: 1900, y: 700, path: [{ x: 1700, y: 700 }, { x: 2100, y: 700 }] },
      { x: 2700, y: 700, path: [{ x: 2500, y: 700 }, { x: 2900, y: 700 }] },
      { x: 3400, y: 700, path: [{ x: 3200, y: 700 }, { x: 3600, y: 700 }] },
      { x: 4200, y: 700, path: [{ x: 4000, y: 700 }, { x: 4400, y: 700 }] },
      { x: 5000, y: 700, path: [{ x: 4800, y: 700 }, { x: 5200, y: 700 }] },
    ];

    for (const cfg of enemyConfigs) {
      const enemy = new Enemy(this, cfg.x, cfg.y, cfg.path);
      this.enemies.add(enemy);
      this.physics.add.collider(enemy, this.platforms);
    }
  }

  // --- Skill Spawning ---

  spawnPlayerSkill(origin, skill) {
    let projectile;
    if (skill.resource === 'fireball') {
      projectile = new Fireball(this, origin, skill);
    } else {
      projectile = new Icepick(this, origin, skill);
    }
    projectile.originType = 'player';
    this.playerSkills.add(projectile);
  }

  spawnEnemySkill(origin, skill) {
    let projectile;
    if (skill.resource === 'fireball') {
      projectile = new Fireball(this, origin, skill);
    } else {
      projectile = new Icepick(this, origin, skill);
    }
    projectile.originType = 'enemy';
    this.enemySkills.add(projectile);
  }

  // --- Collision Handlers ---

  onPlayerSkillHitEnemy(skillSprite, enemy) {
    if (skillSprite.phase === 'dead') return;
    skillSprite.hitTarget(enemy);
  }

  onEnemySkillHitPlayer(player, skillSprite) {
    if (skillSprite.phase === 'dead') return;
    skillSprite.hitTarget(player);
  }

  onPlayerLand(player, platform) {
    // Landing sound (only when coming from above)
    if (player.body.velocity.y >= 0 && player.body.blocked.down) {
      // Handled in player physics
    }
  }

  onPlayerCollectPickup(player, pickup) {
    if (pickup.skillData) {
      player.collectPickup(pickup.skillData);
    }
    pickup.destroy();
  }

  // --- Spawning ---

  spawnPickup(x, y, skill) {
    const pickup = this.physics.add.sprite(x, y, 'pickup').setScale(0.3);
    pickup.skillData = skill;
    pickup.body.setAllowGravity(false);
    pickup.rotationSpeed = -90; // degrees per second
    this.pickups.add(pickup);
    this.physics.add.overlap(this.player, pickup, this.onPlayerCollectPickup, null, this);
    // Rotate pickup in update
    return pickup;
  }

  spawnDamageText(x, y, damage) {
    const dt = new DamageText(this, x, y, damage);
    this.damageTexts.push(dt);
  }

  // --- Pause ---

  togglePause() {
    this.isPaused = !this.isPaused;
    this.pauseText.setVisible(this.isPaused);
    if (this.isPaused) {
      this.physics.world.pause();
    } else {
      this.physics.world.resume();
    }
  }

  // --- Main Update ---

  update(time, delta) {
    if (this.isPaused) return;

    this.elapsedTime += delta / 1000;

    // Update player
    this.player.update(time, delta);

    // Update enemies
    this.enemies.getChildren().forEach(enemy => {
      if (enemy.active) enemy.update(time, delta);
    });

    // Update damage texts
    this.damageTexts = this.damageTexts.filter(dt => dt.update(delta));

    // Update pickups rotation
    this.pickups.getChildren().forEach(pickup => {
      if (pickup.active) {
        pickup.angle -= 90 * (delta / 1000);
      }
    });

    // Snowflakes
    for (const sf of this.snowflakes) {
      sf.y += sf.speed * (delta / 16);
      sf.x += Math.sin(this.elapsedTime * sf.sway) * 0.5;

      if (sf.y > this.cameras.main.scrollY + 800) {
        sf.y = this.cameras.main.scrollY - 20;
        sf.x = Phaser.Math.Between(
          this.cameras.main.scrollX,
          this.cameras.main.scrollX + 1280
        );
      }
    }

    // Update HUD
    this.hud.update(this.player);
  }
}
