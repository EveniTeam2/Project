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
            if (ImpactDuration > 0)
                return;

            WantToMove = isRun;
            TargetSpeed = isRun ? GetSpeed() : 0; // 몬스터는 왼쪽으로 이동
        }

        public override void SetBackward(bool isBack)
        {
            if (ImpactDuration > 0)
                return;

            WantToMove = isBack;
            TargetSpeed = isBack ? GetSpeed() : 0; // 몬스터는 오른쪽으로 이동
        }
    }
}