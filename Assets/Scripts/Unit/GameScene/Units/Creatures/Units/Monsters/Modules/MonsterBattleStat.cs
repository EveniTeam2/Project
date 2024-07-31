using System;
using Unit.GameScene.Units.Creatures.Module.Systems;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules
{
    public class MonsterBattleStat
    {
        private readonly Func<int> _attack;
        private readonly Func<float> _coolTime;

        public MonsterBattleStat(CreatureStat<MonsterStat> creatureStat)
        {
            _attack = () => creatureStat.Current.Attack;
            _coolTime = () => creatureStat.Current.CoolTime;
        }

        public float GetCoolTime()
        {
            return _coolTime();
        }

        public int GetAttack()
        {
            return _attack();
        }
    }
}