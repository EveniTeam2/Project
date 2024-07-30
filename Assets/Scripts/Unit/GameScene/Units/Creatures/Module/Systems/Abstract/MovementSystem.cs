using Unit.GameScene.Units.Creatures.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.Abstract
{
    public abstract class MovementSystem : ICreatureMovement
    {
        // Y movement 관련
        protected const float _gravity = -20f;

        // 속도 댐핑 변수
        protected float _dampTime = 0.3f;
        protected IMovementStat _stats;
        protected readonly Transform _targetTransform;

        // X movement 관련
        protected float _currentSpd;
        protected float _dampVel;
        protected float _groundYPosition;
        protected float _targetSpd;
        protected bool _wantToMove;

        // 점프 관련
        protected float _currentYSpd;
        protected bool _wantToJump;

        // 움직임 판단
        public bool IsInAir => _targetTransform.position.y > _groundYPosition;
        public bool IsMoving => _currentSpd * _currentSpd > 0;

        // 임팩트
        protected float _impactDuration;

        public MovementSystem(Transform targetTransform, IMovementStat stats, float ground)
        {
            _targetTransform = targetTransform;
            _groundYPosition = ground;
            _stats = stats;
        }

        public virtual void Update()
        {
            _impactDuration -= Time.deltaTime;
            Vector2 pos = _targetTransform.position;

            // 달리기
            if (IsMoving || _wantToMove)
            {
                _currentSpd = CalculateSpeed(_currentSpd, _targetSpd);
                pos.x += _currentSpd * Time.deltaTime;
            }

            // 점프
            if (IsInAir || _wantToJump)
            {
                pos.y += _currentYSpd * Time.deltaTime;
                if (pos.y < _groundYPosition)
                    pos.y = _groundYPosition;
                _wantToJump = false;
            }
            _targetTransform.position = pos;
        }

        public virtual void FixedUpdate()
        {
            if (IsMoving || _wantToMove)
                _currentSpd = CalculateSpeed(_currentSpd, _targetSpd);
            JumpFixedUpdate();
        }

        public abstract void SetRun(bool isRun);
        public abstract void SetBackward(bool isBackward);

        public virtual void SetGroundPosition(float groundYPosition)
        {
            _groundYPosition = groundYPosition;
        }

        protected virtual float CalculateSpeed(float currentSpd, float targetSpd)
        {
            var value = currentSpd - targetSpd;
            if (value * value > 0.0001f)
                currentSpd = Mathf.SmoothDamp(currentSpd, targetSpd, ref _dampVel, _dampTime);
            else
                currentSpd = targetSpd;
            return currentSpd;
        }

        public virtual void Jump(float power)
        {
            if (_impactDuration > 0)
                return;

            _wantToJump = true;
            _currentYSpd = power;
        }

        protected void JumpFixedUpdate()
        {
            _currentYSpd += _gravity * Time.fixedDeltaTime;
        }

        public virtual void SetImpact(Vector2 impact, float duration)
        {
            _currentSpd += impact.x;
            _currentYSpd += impact.y;
            _targetSpd = 0;
            _impactDuration = duration;
        }

        public abstract void SpawnInit(IMovementStat movementStat);
    }

    public interface IMovementStat
    {
        int GetSpeed();
    }
}