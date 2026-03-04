import Phaser from 'phaser';

export class StartMenuScene extends Phaser.Scene {
  constructor() {
    super('StartMenuScene');
  }

  create() {
    const { width, height } = this.cameras.main;

    // Background
    this.add.image(width / 2, height / 2, 'menu-bg').setDisplaySize(width, height);

    // Star cluster & sun decorations
    this.add.image(width * 0.2, height * 0.3, 'menu-starcluster').setScale(0.5);
    this.add.image(width * 0.8, height * 0.2, 'menu-sun').setScale(0.4);

    // Title
    this.add.image(width / 2, height * 0.25, 'menu-title').setScale(0.6);

    // Snowflakes in background
    this.snowflakes = [];
    for (let i = 0; i < 15; i++) {
      const sf = this.add.image(
        Phaser.Math.Between(0, width),
        Phaser.Math.Between(-100, height),
        'menu-snowflake'
      );
      sf.setScale(Phaser.Math.FloatBetween(0.1, 0.3));
      sf.setAlpha(Phaser.Math.FloatBetween(0.3, 0.7));
      sf.speed = Phaser.Math.FloatBetween(0.3, 1.0);
      sf.sway = Phaser.Math.FloatBetween(0.5, 2.0);
      this.snowflakes.push(sf);
    }

    // Menu options
    this.selected = 0;
    this.options = [];

    const startY = height * 0.55;
    const spacing = 70;

    // Start option
    this.startNormal = this.add.image(width / 2, startY, 'menu-start').setScale(0.5);
    this.startSelected = this.add.image(width / 2, startY, 'menu-start-selected').setScale(0.5);
    this.options.push({ normal: this.startNormal, selected: this.startSelected });

    // Quit option
    this.quitNormal = this.add.image(width / 2, startY + spacing, 'menu-quit').setScale(0.5);
    this.quitSelected = this.add.image(width / 2, startY + spacing, 'menu-quit-selected').setScale(0.5);
    this.options.push({ normal: this.quitNormal, selected: this.quitSelected });

    this.updateMenuDisplay();

    // Input
    this.keyW = this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.W);
    this.keyS = this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.S);
    this.keyUp = this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.UP);
    this.keyDown = this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.DOWN);
    this.keyEnter = this.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.ENTER);

    // Background music
    if (!this.sound.get('bgm-mirage')) {
      this.bgm = this.sound.add('bgm-mirage', { loop: true, volume: 0.3 });
      this.bgm.play();
    }

    this.elapsedTime = 0;
  }

  updateMenuDisplay() {
    this.options.forEach((opt, index) => {
      opt.normal.setVisible(index !== this.selected);
      opt.selected.setVisible(index === this.selected);
    });
  }

  update(time, delta) {
    this.elapsedTime += delta / 1000;

    // Snowflake animation
    const { width, height } = this.cameras.main;
    for (const sf of this.snowflakes) {
      sf.y += sf.speed * (delta / 16);
      sf.x += Math.sin(this.elapsedTime * sf.sway) * 0.5;

      if (sf.y > height + 20) {
        sf.y = -20;
        sf.x = Phaser.Math.Between(0, width);
      }
    }

    // Menu navigation
    if (Phaser.Input.Keyboard.JustDown(this.keyW) || Phaser.Input.Keyboard.JustDown(this.keyUp)) {
      this.selected = this.selected <= 0 ? this.options.length - 1 : this.selected - 1;
      this.updateMenuDisplay();
    }

    if (Phaser.Input.Keyboard.JustDown(this.keyS) || Phaser.Input.Keyboard.JustDown(this.keyDown)) {
      this.selected = this.selected >= this.options.length - 1 ? 0 : this.selected + 1;
      this.updateMenuDisplay();
    }

    if (Phaser.Input.Keyboard.JustDown(this.keyEnter)) {
      if (this.selected === 0) {
        // Stop menu music
        this.sound.stopAll();
        this.scene.start('GameScene');
      }
      // Quit not applicable in browser — could show message
    }
  }
}
