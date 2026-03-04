export class DamageText {
  constructor(scene, x, y, damage) {
    this.text = scene.add.text(x, y - 20, Math.round(damage).toString(), {
      fontSize: '18px',
      fill: '#ff4444',
      fontStyle: 'bold',
      stroke: '#000000',
      strokeThickness: 2,
    }).setDepth(100).setOrigin(0.5);

    this.velocityX = 30;
    this.velocityY = -200;

    // Auto-destroy after 1 second
    scene.time.delayedCall(1000, () => {
      if (this.text) {
        this.text.destroy();
        this.text = null;
      }
    });

    this.scene = scene;
  }

  update(delta) {
    if (!this.text) return false;

    const dt = delta / 1000;
    this.text.x += this.velocityX * dt;
    this.text.y += this.velocityY * dt;

    // Gravity
    this.velocityY += 500 * dt;

    // Damping
    this.velocityX *= 0.95;
    this.velocityY *= 0.95;

    // Fade out
    this.text.setAlpha(this.text.alpha - dt * 0.5);

    return this.text !== null;
  }
}
