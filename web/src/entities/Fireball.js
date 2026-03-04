import Phaser from 'phaser';

export class Fireball extends Phaser.Physics.Arcade.Sprite {
  constructor(scene, origin, skill) {
    super(scene, origin.x, origin.y, 'fire1');
    scene.add.existing(this);
    scene.physics.add.existing(this);

    this.setScale(0.4);
    this.body.setAllowGravity(false);
    this.body.setSize(40, 40);

    this.skill = skill;
    this.origin = origin;
    this.direction = origin.direction || 1;
    this.setFlipX(this.direction < 0);

    // Lifecycle
    this.phase = 'startup';
    this.phaseTimer = 0;

    this.play('fireball-anim');
    this.scene.sound.play('sfx-fire', { volume: 0.4 });

    // Set velocity after startup
    scene.time.delayedCall(skill.startupTime * 1000, () => {
      if (this.active) {
        this.phase = 'active';
        this.body.velocity.x = skill.speed * this.direction;
      }
    });

    // Auto-destroy after active time
    scene.time.delayedCall((skill.startupTime + skill.activeTime) * 1000, () => {
      if (this.active) {
        this.destroy();
      }
    });
  }

  hitTarget(target) {
    if (this.phase === 'dead') return;
    this.phase = 'dead';

    target.applyDamage(this.skill.magnitude);
    this.scene.spawnDamageText(this.x, this.y, this.skill.magnitude);
    this.destroy();
  }
}
