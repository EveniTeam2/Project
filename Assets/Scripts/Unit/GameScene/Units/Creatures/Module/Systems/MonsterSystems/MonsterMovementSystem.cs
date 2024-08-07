using Unit.GameScene.Units.Creatures.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems
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
            WantToMove = isRun;
            if (ImpactDuration > 0)
            {
                DelayOrder = (new MovementOrder(SetRun, isRun));
                return;
            }
            DelayOrder = null;
            TargetSpeed = isRun ? -GetSpeed() : 0; // 몬스터는 왼쪽으로 이동
        }

        public override void SetBackward(bool isBack)
        {
            WantToMove = isBack;
            if (ImpactDuration > 0)
            {
                DelayOrder = (new MovementOrder(SetBackward, isBack));
                return;
            }
            DelayOrder = null;
            TargetSpeed = isBack ? GetSpeed() : 0; // 몬스터는 오른쪽으로 이동
        }
    }
}