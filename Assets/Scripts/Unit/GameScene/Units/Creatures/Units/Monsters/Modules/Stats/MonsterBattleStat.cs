using System;
using Unit.GameScene.Units.Creatures.Interfaces.Stats;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Stats
{
    public class MonsterBattleStat : IBattleStat
    {
        private readonly Func<int> _damage;
        private readonly Func<float> _coolTime;

        public MonsterBattleStat(MonsterStatSystem monsterStatSystem)
        {
            _damage = () => monsterStatSystem.Damage;
            _coolTime = () => monsterStatSystem.AttackCoolTime;
        }

        public float GetCoolTime()
        {
            return _coolTime();
        }

        public int GetDamage()
        {
            return _damage();
        }
    }
}