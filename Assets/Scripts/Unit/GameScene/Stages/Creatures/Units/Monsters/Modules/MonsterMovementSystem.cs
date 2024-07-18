using System;
using Unit.GameScene.Stages.Creatures.Module;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Monsters.Modules
{
    public class MonsterMovementSystem : MovementSystem
    {
        public MonsterMovementSystem(Transform targetTransform, MonsterMovementStat stats, float ground) : base(
            targetTransform, stats, ground)
        {
        }

        public override void SetRun(bool isRun)
        {
            _wantToMove = isRun;
            if (isRun)
                _targetSpd = -1 * _stats.GetSpeed();
            else
                _targetSpd = 0;
        }

        public override void SetBackward(bool isBack)
        {
            _wantToMove = isBack;
            if (_wantToMove)
                _targetSpd = _stats.GetSpeed();
            else
                _targetSpd = 0;
        }

        public override void Jump(float power)
        {
        }
    }

    public class MonsterMovementStat : IMovementStat
    {
        protected Func<int> _getSpeed;

        public MonsterMovementStat(Stat<MonsterStat> stats)
        {
            _getSpeed = () => stats.Current.Speed;
        }

        public int GetSpeed()
        {
            return _getSpeed();
        }
    }
}