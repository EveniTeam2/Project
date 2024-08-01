using Unit.GameScene.Units.Creatures.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems
{
    public class MonsterMovementSystem : MovementSystem
    {
        private readonly MonsterStatSystem _monsterStatSystem;

        public MonsterMovementSystem(MonsterStatSystem monsterStatSystem, Transform targetTransform, float ground)
            : base(targetTransform, ground)
        {
            _monsterStatSystem = monsterStatSystem;
        }

        protected override int GetSpeed() => _monsterStatSystem.Speed;

        public override void SetRun(bool isRun)
        {
            if (_impactDuration > 0)
                return;

            _wantToMove = isRun;
            _targetSpeed = isRun ? -GetSpeed() : 0;
        }

        public override void SetBackward(bool isBack)
        {
            if (_impactDuration > 0)
                return;

            _wantToMove = isBack;
            _targetSpeed = isBack ? GetSpeed() : 0;
        }
    }
}