// Ported from EnemyData.cs

const BASE_HEALTH = 95;
const HEALTH_INCREMENT = 5;
const BASE_ENERGY = 45;
const ENERGY_INCREMENT = 5;

export class EnemyData {
  constructor(level = 1) {
    this.level = level;
    this.maxHealth = BASE_HEALTH + (level * HEALTH_INCREMENT);
    this.maxEnergy = BASE_ENERGY + (level * ENERGY_INCREMENT);
  }

  get maxWanderSpeed() { return 80; } // scaled from 500
  get viewRangeX() { return 350; } // scaled from 700
  get viewRangeY() { return 150; } // scaled from 300
  get minNodeDistance() { return 40; } // scaled from 200
  get nodeWaitTime() { return 200; } // ms (was 0.2s)
}
