using ScriptableObjects.Scripts.Creature;
using Unit.Stages.Creatures.FSM;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.Stages.Creatures.Monsters
{
    public class Monster : BaseCreature
    {
        private Stat<MonsterStat> _stats;
        [FormerlySerializedAs("_characterStateData")] [SerializeField] private CreatureStateData creatureStateData;
        public override int Health { get => _stats.Current.Health; protected set => _stats.SetCurrent(new MonsterStat { Attack = _stats.Current.Attack, Health = value }); }
        public override Animator Animator => _animator;
        [SerializeField] private Animator _animator;

        public void Initialize(MonsterStat stat)
        {
            _stats = new Stat<MonsterStat>(stat);
            HFSM = StateBuilder.BuildState(this, creatureStateData);
        }

        public override void SetHealth(int health)
        {
            _stats.SetOrigin(new MonsterStat { Attack = _stats.Origin.Attack, Health = health });
            _stats.SetCurrent(new MonsterStat { Attack = _stats.Current.Attack, Health = health });
        }
    }
}