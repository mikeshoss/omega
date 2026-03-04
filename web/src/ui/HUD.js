import Phaser from 'phaser';

export class HUD {
  constructor(scene) {
    this.scene = scene;

    const barY = 20;
    const barX = 20;
    const barWidth = 200;
    const barHeight = 16;

    // Action bar frame background
    this.actionBar = scene.add.image(barX + barWidth / 2, barY + 50, 'actionbar')
      .setScale(0.4)
      .setScrollFactor(0)
      .setDepth(90);

    // Health bar background frame
    this.healthFrame = scene.add.image(barX + barWidth / 2, barY + 10, 'hudbarframe')
      .setDisplaySize(barWidth + 8, barHeight + 8)
      .setScrollFactor(0)
      .setDepth(90);

    // Health bar fill
    this.healthBar = scene.add.graphics().setScrollFactor(0).setDepth(91);

    // Energy bar background frame
    this.energyFrame = scene.add.image(barX + barWidth / 2, barY + 34, 'hudbarframe')
      .setDisplaySize(barWidth + 8, barHeight + 8)
      .setScrollFactor(0)
      .setDepth(90);

    // Energy bar fill
    this.energyBar = scene.add.graphics().setScrollFactor(0).setDepth(91);

    // Skill icons
    this.skillIcons = [];
    const iconStartX = barX + barWidth + 30;
    const iconY = barY + 20;
    const iconSpacing = 40;

    for (let i = 0; i < 4; i++) {
      const icon = scene.add.image(iconStartX + i * iconSpacing, iconY, 'skill-noskill')
        .setScale(0.35)
        .setScrollFactor(0)
        .setDepth(90);
      this.skillIcons.push(icon);
    }

    // Selection highlight
    this.selectionHighlight = scene.add.rectangle(
      iconStartX, iconY, 36, 36, 0xffffff, 0.3
    ).setScrollFactor(0).setDepth(89);

    this.barX = barX;
    this.barY = barY;
    this.barWidth = barWidth;
    this.barHeight = barHeight;
    this.iconStartX = iconStartX;
    this.iconY = iconY;
    this.iconSpacing = iconSpacing;

    // Label text
    this.healthLabel = scene.add.text(barX + 4, barY + 3, 'HP', {
      fontSize: '11px', fill: '#ffffff', fontStyle: 'bold',
    }).setScrollFactor(0).setDepth(92);

    this.energyLabel = scene.add.text(barX + 4, barY + 27, 'EP', {
      fontSize: '11px', fill: '#ffffff', fontStyle: 'bold',
    }).setScrollFactor(0).setDepth(92);
  }

  update(player) {
    const healthPct = Math.max(0, player.health / player.maxHealth);
    const energyPct = Math.max(0, player.energy / player.maxEnergy);

    // Health bar
    this.healthBar.clear();
    this.healthBar.fillStyle(0x22cc22, 1);
    this.healthBar.fillRect(this.barX + 4, this.barY + 4, (this.barWidth - 8) * healthPct, this.barHeight - 6);

    // Energy bar
    this.energyBar.clear();
    this.energyBar.fillStyle(0x2266ee, 1);
    this.energyBar.fillRect(this.barX + 4, this.barY + 28, (this.barWidth - 8) * energyPct, this.barHeight - 6);

    // Update skill icons
    for (let i = 0; i < 4; i++) {
      if (i < player.selectedSkills.length && player.selectedSkills[i]) {
        const skillName = player.selectedSkills[i].resource;
        if (skillName === 'fireball') {
          this.skillIcons[i].setTexture('skill-fireball');
        } else if (skillName === 'icepick') {
          this.skillIcons[i].setTexture('skill-icepick');
        }
      } else {
        this.skillIcons[i].setTexture('skill-noskill');
      }
    }

    // Update selection highlight
    this.selectionHighlight.setPosition(
      this.iconStartX + player.currentSkill * this.iconSpacing,
      this.iconY
    );
  }
}
