import Phaser from 'phaser';
import { BootScene } from './scenes/BootScene.js';
import { StartMenuScene } from './scenes/StartMenuScene.js';
import { GameScene } from './scenes/GameScene.js';

const config = {
  type: Phaser.AUTO,
  width: 1280,
  height: 720,
  parent: 'game-container',
  backgroundColor: '#1a1a2e',
  physics: {
    default: 'arcade',
    arcade: {
      gravity: { y: 800 },
      debug: false,
    },
  },
  scene: [BootScene, StartMenuScene, GameScene],
};

new Phaser.Game(config);
