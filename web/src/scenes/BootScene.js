import Phaser from 'phaser';

export class BootScene extends Phaser.Scene {
  constructor() {
    super('BootScene');
  }

  preload() {
    // Loading bar
    const width = this.cameras.main.width;
    const height = this.cameras.main.height;
    const barWidth = 400;
    const barHeight = 30;
    const barX = (width - barWidth) / 2;
    const barY = height / 2;

    const progressBar = this.add.graphics();
    const progressBox = this.add.graphics();
    progressBox.fillStyle(0x222222, 0.8);
    progressBox.fillRect(barX, barY, barWidth, barHeight);

    const loadingText = this.add.text(width / 2, barY - 30, 'Loading...', {
      fontSize: '20px', fill: '#ffffff',
    }).setOrigin(0.5);

    this.load.on('progress', (value) => {
      progressBar.clear();
      progressBar.fillStyle(0x3498db, 1);
      progressBar.fillRect(barX + 5, barY + 5, (barWidth - 10) * value, barHeight - 10);
    });

    this.load.on('complete', () => {
      progressBar.destroy();
      progressBox.destroy();
      loadingText.destroy();
    });

    // --- PLAYER TEXTURES ---
    this.load.image('player-idle1', 'textures/player/idle1.png');
    this.load.image('player-idle2', 'textures/player/idle2.png');
    for (let i = 1; i <= 6; i++) {
      this.load.image(`player-run${i}`, `textures/player/ninjarun_${i}.png`);
    }
    for (let i = 1; i <= 7; i++) {
      this.load.image(`player-jump${i}`, `textures/player/ninjajump_${i}.png`);
    }

    // --- FIREBALL TEXTURES ---
    for (let i = 1; i <= 7; i++) {
      this.load.image(`fire${i}`, `textures/skills/fire${i}.png`);
    }
    this.load.image('fireball', 'textures/skills/fireball.png');

    // --- ICEPICK TEXTURES ---
    for (let i = 1; i <= 8; i++) {
      this.load.image(`snow${i}`, `textures/skills/snow${i}.png`);
    }
    this.load.image('iceball', 'textures/skills/iceball.png');

    // --- TERRAIN ---
    this.load.image('ice-ground', 'textures/terrain/ice-ground-001.png');
    this.load.image('ice-ground-top', 'textures/terrain/ice-ground-top-001.png');
    this.load.image('ice-platform', 'textures/terrain/ice-platform-001.png');

    // --- UI ---
    this.load.image('actionbar', 'textures/ui/actionbar.png');
    this.load.image('energybar', 'textures/ui/energybar.png');
    this.load.image('healthbar', 'textures/ui/healthbar.png');
    this.load.image('hudbarframe', 'textures/ui/hudbarframe.png');
    this.load.image('skill-fireball', 'textures/ui/skill-fireball.png');
    this.load.image('skill-icepick', 'textures/ui/skill-icepick.png');
    this.load.image('skill-noskill', 'textures/ui/skill-noskill.png');

    // --- MENU ---
    this.load.image('menu-bg', 'textures/menu/startmenu-background.png');
    this.load.image('menu-title', 'textures/menu/title.png');
    this.load.image('menu-container', 'textures/menu/container.png');
    this.load.image('menu-starcluster', 'textures/menu/starcluster.png');
    this.load.image('menu-sun', 'textures/menu/sun.png');
    this.load.image('menu-snowflake', 'textures/menu/snowflake.png');
    this.load.image('menu-start', 'textures/menu/menuoption-start.png');
    this.load.image('menu-start-selected', 'textures/menu/menuoption-start-selected.png');
    this.load.image('menu-quit', 'textures/menu/menuoption-quit.png');
    this.load.image('menu-quit-selected', 'textures/menu/menuoption-quit-selected.png');

    // --- EFFECTS ---
    this.load.image('crossbones', 'textures/effects/Crossbones.png');
    this.load.image('pickup', 'textures/effects/pickup.png');

    // --- AUDIO ---
    this.load.audio('bgm-mirage', 'sounds/mirage.mp3');
    this.load.audio('bgm-ninja', 'sounds/ninja-gaiden-act4-2.mp3');
    this.load.audio('bgm-summit', 'sounds/summit.mp3');
    this.load.audio('sfx-jump', 'sounds/player-jump-tackoff.mp3');
    this.load.audio('sfx-step1', 'sounds/snowstep1.mp3');
    this.load.audio('sfx-step2', 'sounds/snowstep2.mp3');
    this.load.audio('sfx-enemy-die', 'sounds/enemy-die.wav');
    this.load.audio('sfx-enemy-hit', 'sounds/enemy-hit.wav');
    this.load.audio('sfx-fire', 'sounds/fire.wav');
    this.load.audio('sfx-ice', 'sounds/ice.wav');
    this.load.audio('sfx-pickup', 'sounds/pickupSound.wav');
    this.load.audio('sfx-player-land', 'sounds/player-land.wav');
  }

  create() {
    // Create animations
    this.anims.create({
      key: 'player-idle',
      frames: [{ key: 'player-idle1' }, { key: 'player-idle2' }],
      frameRate: 4,
      repeat: -1,
    });

    this.anims.create({
      key: 'player-run',
      frames: [
        { key: 'player-run1' }, { key: 'player-run2' }, { key: 'player-run3' },
        { key: 'player-run4' }, { key: 'player-run5' }, { key: 'player-run6' },
      ],
      frameRate: 10,
      repeat: -1,
    });

    this.anims.create({
      key: 'player-jump',
      frames: [
        { key: 'player-jump1' }, { key: 'player-jump2' }, { key: 'player-jump3' },
        { key: 'player-jump4' }, { key: 'player-jump5' }, { key: 'player-jump6' },
        { key: 'player-jump7' },
      ],
      frameRate: 10,
      repeat: 0,
    });

    this.anims.create({
      key: 'fireball-anim',
      frames: [
        { key: 'fire1' }, { key: 'fire2' }, { key: 'fire3' }, { key: 'fire4' },
        { key: 'fire5' }, { key: 'fire6' }, { key: 'fire7' },
      ],
      frameRate: 12,
      repeat: -1,
    });

    this.anims.create({
      key: 'icepick-anim',
      frames: [
        { key: 'snow1' }, { key: 'snow2' }, { key: 'snow3' }, { key: 'snow4' },
        { key: 'snow5' }, { key: 'snow6' }, { key: 'snow7' }, { key: 'snow8' },
      ],
      frameRate: 12,
      repeat: -1,
    });

    this.scene.start('StartMenuScene');
  }
}
