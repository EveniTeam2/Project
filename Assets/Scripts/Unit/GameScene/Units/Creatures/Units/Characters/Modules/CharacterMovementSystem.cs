using System;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Modules
{
    public class CharacterMovementSystem : MovementSystem
    {
        protected float _boostSpeed;

        // 속도 부스트
        protected float _boostTimer;

        // 점프 관련
        protected float _currentYSpd;
        protected bool _wantToJump;

        public CharacterMovementSystem(Transform targetTransform, CharacterMovementStat stats, float ground) : base(
            targetTransform, stats, ground)
        {
        }

        public override void Update()
        {
            base.Update();
            Vector2 pos = _targetTransform.position;

            if (IsInAir || _wantToJump)
            {
                pos.y += _currentYSpd * Time.deltaTime;
                if (pos.y < _groundYPosition)
                    pos.y = _groundYPosition;
                _wantToJump = false;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            JumpFixedUpdate();
        }

        public override void SetRun(bool isRun)
        {
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

        public override void Jump(float power)
        {
            _wantToJump = true;
            _currentYSpd = power;
        }

        public void SetSpeedBoost(float boost, float duration)
        {
            // TODO
        }

        protected void JumpFixedUpdate()
        {
            _currentYSpd += _gravity * Time.fixedDeltaTime;
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