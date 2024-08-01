using System;
using Unit.GameScene.Units.Creatures.Interfaces.Movements;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class MovementSystem : ICreatureMovement
    {
        // 상수
        protected const float Gravity = -20f;

        // Transform 및 위치 관련
        protected readonly Transform _targetTransform;
        protected float _groundYPosition;

        // 이동 관련
        protected float _currentSpeed;
        protected float _targetSpeed;
        protected float _dampVelocity;
        protected float _dampTime = 0.3f;

        // 원래 속도 저장
        private float _originalSpeed;

        // 점프 관련
        protected float _currentYSpeed;
        protected bool _wantToJump;
        protected bool _wantToMove;

        // 충격(넉백) 관련
        protected float _impactDuration;

        // 속도 부스트 관련
        protected float _boostSpeed;
        protected float _boostTimer;

        // 이동 상태 확인을 위한 속성
        public bool IsInAir => _targetTransform.position.y > _groundYPosition;
        public bool IsMoving => Mathf.Abs(_currentSpeed) > 0.0001f;

        protected MovementSystem(Transform targetTransform, float groundYPosition)
        {
            _targetTransform = targetTransform ?? throw new ArgumentNullException(nameof(targetTransform));
            _groundYPosition = groundYPosition;
        }

        public virtual void Update()
        {
            ApplyImpact();
            ApplySpeedBoost();
            UpdatePosition();
        }

        public void FixedUpdate()
        {
            if (IsMoving || _wantToMove)
            {
                _currentSpeed = CalculateSpeed(_currentSpeed, _targetSpeed);
            }

            UpdateVerticalSpeed();
        }

        // 하위 클래스에서 구현할 추상 메서드
        protected abstract int GetSpeed();
        public abstract void SetRun(bool isRunning);
        public abstract void SetBackward(bool isBackward);

        // 이동 시스템을 제어하는 공용 메서드
        public virtual void SetGroundPosition(float groundYPosition)
        {
            _groundYPosition = groundYPosition;
        }

        public virtual void Jump(float power)
        {
            if (_impactDuration <= 0)
            {
                _wantToJump = true;
                _currentYSpeed = power;
            }
        }

        public void SetImpact(Vector2 impact, float duration)
        {
            // 현재 속도를 저장하고 충격을 가합니다.
            _originalSpeed = _targetSpeed;
            _currentSpeed -= impact.x;
            _currentYSpeed += impact.y;
            _targetSpeed = 0;
            _impactDuration = duration;
        }

        private void ApplyImpact()
        {
            _impactDuration -= Time.deltaTime;
            
            if (_impactDuration <= 0 && !IsMoving)
            {
                RestoreOriginalSpeed();
            }
        }

        private void RestoreOriginalSpeed()
        {
            // 원래 속도로 복구
            _targetSpeed = _originalSpeed;
            _originalSpeed = 0;
        }

        private void ApplySpeedBoost()
        {
            if (_boostTimer > 0)
            {
                _boostTimer -= Time.deltaTime;
                _currentSpeed += _boostSpeed;
            }
            else
            {
                _boostSpeed = 0;
            }
        }

        private void UpdatePosition()
        {
            var position = (Vector2)_targetTransform.position;

            // 수평 이동 처리
            if (IsMoving || _wantToMove)
            {
                _currentSpeed = CalculateSpeed(_currentSpeed, _targetSpeed);
                position.x += _currentSpeed * Time.deltaTime;
            }

            // 수직 이동 처리 (점프)
            if (IsInAir || _wantToJump)
            {
                position.y += _currentYSpeed * Time.deltaTime;
                if (position.y < _groundYPosition)
                {
                    position.y = _groundYPosition;
                    _wantToJump = false;
                }
            }

            _targetTransform.position = position;
        }

        private float CalculateSpeed(float currentSpeed, float targetSpeed)
        {
            float difference = currentSpeed - targetSpeed;
            if (Mathf.Abs(difference) > 0.0001f)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref _dampVelocity, _dampTime);
            }
            else
            {
                currentSpeed = targetSpeed;
            }
            return currentSpeed;
        }

        private void UpdateVerticalSpeed()
        {
            _currentYSpeed += Gravity * Time.fixedDeltaTime;
        }
    }
}
