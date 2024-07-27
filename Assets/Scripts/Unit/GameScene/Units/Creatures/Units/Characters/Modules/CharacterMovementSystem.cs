using System;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterMovementSystem : MovementSystem
    {
        // 속도 부스트
        protected float _boostSpeed;
        protected float _boostTimer;

        public CharacterMovementSystem(Transform targetTransform, CharacterMovementStat stats, float ground) : base(
            targetTransform, stats, ground)
        {
        }

        public override void SetRun(bool isRun)
        {
            if (_impactDuration > 0)
                return;

            _wantToMove = isRun;
            if (isRun)
                _targetSpd = _stats.GetSpeed();
            else
                _targetSpd = 0;
        }

        public override void SetBackward(bool isBack)
        {
            _wantToMove = isBack;
            if (_wantToMove)
                _targetSpd = -1 * _stats.GetSpeed();
            else
                _targetSpd = 0;
        }

        public void SetSpeedBoost(float boost, float duration)
        {
            // TODO
        }

        private void SpeedBoost()
        {
            // TODO
        }

        public override void SpawnInit(IMovementStat movementStat)
        {
            _stats = movementStat;
        }
    }

    public class CharacterMovementStat : IMovementStat
    {
        protected Func<int> _getSpeed;

        public CharacterMovementStat(Stat<CharacterStat> stats)
        {
            _getSpeed = () => stats.Current.Speed;
        }

        public int GetSpeed()
        {
            return _getSpeed();
        }
    }
}