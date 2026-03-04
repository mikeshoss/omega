// Ported from Skill.cs

export class Skill {
  constructor(id, name, description, baseMagnitude, config = {}) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.level = 1;
    this.baseMagnitude = baseMagnitude;
    this.magnitude = baseMagnitude;

    this.originDelay = config.originDelay || 0.6;   // seconds
    this.startupTime = config.startupTime || 0.2;   // seconds
    this.activeTime = config.activeTime || 3.0;     // seconds
    this.endTime = config.endTime || 0.1;           // seconds
    this.speed = config.speed || 340;               // pixels/sec (scaled from Unity)
    this.resource = config.resource || 'fireball';
  }

  levelUp() {
    this.level++;
    this.magnitude = this.baseMagnitude * (1 + (this.level / 10));
  }
}

// Skill factory functions
export function createFireball() {
  const skill = new Skill(0, 'Fireball', 'A blazing fireball', 30, {
    originDelay: 0.6,
    startupTime: 0.2,
    activeTime: 3.0,
    endTime: 0.1,
    speed: 680, // 3400 / 5
    resource: 'fireball',
  });
  // Level up 4 times like in Unity Start()
  for (let i = 0; i < 4; i++) skill.levelUp();
  return skill;
}

export function createIcepick() {
  return new Skill(1, 'Icepick', 'A freezing ice shard', 20, {
    originDelay: 0.6,
    startupTime: 0.1,
    activeTime: 3.0,
    endTime: 0.1,
    speed: 400, // 2000 / 5
    resource: 'icepick',
  });
}

// Enemy uses Icepick with different timings
export function createEnemyIcepick() {
  return new Skill(1, 'Icepick', 'Enemy ice shard', 20, {
    originDelay: 1.5,
    startupTime: 0.3,
    activeTime: 3.0,
    endTime: 1.0,
    speed: 400,
    resource: 'icepick',
  });
}
