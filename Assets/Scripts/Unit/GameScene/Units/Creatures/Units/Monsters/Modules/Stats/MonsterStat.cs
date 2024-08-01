using System;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Stats
{
    [Serializable]
    public struct MonsterStat
    {
        public int damage;
        public int health;
        public int speed;
        public float attackCoolTime;
    }
}