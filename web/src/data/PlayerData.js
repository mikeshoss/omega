// Ported from PlayerData.cs
// Physics values scaled from Unity (divided by ~5) to work with Phaser Arcade Physics

const LEVEL_CAPS = [0, 10, 30, 60, 100, 150, 210, 280, 370, 500];

const LEVEL_MAX = 10;
const JUMP_MAX = 2;
const JUMP_WAIT_TIME = 400; // ms (was 0.4s)
const JUMP_VELOCITY = -420; // negative = upward in Phaser (was 2100 / 5)
const AIR_RUN_ACCEL = 400; // was 20000 / 50
const RUN_ACCEL = 1000; // was 50000 / 50
const MAX_RUN_SPEED = 200; // was 1000 / 5
const BASE_HEALTH = 95;
const HEALTH_INCREMENT = 5;
const BASE_ENERGY = 45;
const ENERGY_INCREMENT = 5;

export class PlayerData {
  constructor(level = 1) {
    this.level = level;
    this.maxHealth = BASE_HEALTH + (level * HEALTH_INCREMENT);
    this.maxEnergy = BASE_ENERGY + (level * ENERGY_INCREMENT) + 50;
    this.exp = 0;
    this.requiredExp = LEVEL_CAPS[level] || 999;
  }

  get maxJump() { return JUMP_MAX; }
  get jumpWaitTime() { return JUMP_WAIT_TIME; }
  get jumpVelocity() { return JUMP_VELOCITY; }
  get airRunAccel() { return AIR_RUN_ACCEL; }
  get runAccel() { return RUN_ACCEL; }
  get maxRunSpeed() { return MAX_RUN_SPEED; }
  get healthRegen() { return 2; } // per second
  get energyRegen() { return 4; } // per second
}
