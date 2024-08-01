using Unit.GameScene.Units.Creatures.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems
{
    public class MonsterMovementSystem : MovementSystem
    {
        private readonly MonsterStatSystem _monsterStatSystem;
        
        public MonsterMovementSystem(Transform targetTransform, MonsterStatSystem monsterStatSystem, float ground) : base(
            targetTransform, ground)
        {
            _monsterStatSystem = monsterStatSystem;
        }

        public override void SetRun(bool isRun)
        {
            if (_impactDuration > 0)
                return;

            _wantToMove = isRun;
            if (isRun)
                _targetSpd = -1 * _monsterStatSystem.Speed;
            else
                _targetSpd = 0;
        }

        public override void SetBackward(bool isBack)
        {
            _wantToMove = isBack;
            if (_wantToMove)
                _targetSpd = _monsterStatSystem.Speed;
            else
                _targetSpd = 0;
        }
    }
}