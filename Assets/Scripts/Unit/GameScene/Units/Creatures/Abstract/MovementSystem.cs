using System;
using Unit.GameScene.Units.Creatures.Interfaces.Movements;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class MovementSystem : ICreatureMovement
    {
        // 상수
        private const float Gravity = -20f;

        // Transform 및 위치 관련
        private readonly Transform _targetTransform;
        private float _groundYPosition;

        // 이동 관련
        private float _currentSpeed;
        protected float TargetSpeed;
        private float _dampVelocity;
        private const float DampTime = 0.3f;

        // 점프 관련
        private float _currentYSpeed;
        private bool _wantToJump;
        protected bool WantToMove;

        // 충격(넉백) 관련
        protected float ImpactDuration;

        // 속도 부스트 관련
        protected float BoostSpeed;
        protected float BoostTimer;

        // 이동 상태 확인을 위한 속성
        private bool IsInAir => _targetTransform.position.y > _groundYPosition;
        private bool IsMoving => Mathf.Abs(_currentSpeed) > 0.0001f;

        protected MovementSystem(Transform targetTransform, float groundYPosition)
        {
            _targetTransform = targetTransform ?? throw new ArgumentNullException(nameof(targetTransform));
            _groundYPosition = groundYPosition;
        }

        public void Update()
        {
            ApplyImpact();
            ApplySpeedBoost();
            UpdatePosition();
        }

        public void FixedUpdate()
        {
            if (IsMoving || WantToMove)
            {
                _currentSpeed = CalculateSpeed(_currentSpeed, TargetSpeed);
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
            if (ImpactDuration <= 0)
            {
                _wantToJump = true;
                _currentYSpeed = power;
            }
        }

        // TODO : 채이환 - [Bug]
        // 캐릭터가 히트한 이후, 적과 거리가 멀어서 RunState로 진입했을 때, 캐릭터가 움직이지 않습니다. RunState 진입은 확인했고, WantToRun 인자 true로 바꾸는 것까지 확인했습니다.
        public void SetImpact(float duration)
        {
            var impact = GetRandomImpactValue();
            _currentSpeed -= impact.x;
            _currentYSpeed += impact.y;
            TargetSpeed = 0;
            ImpactDuration = duration;
        }

        private Vector2 GetRandomImpactValue()
        {
            var impactX = Random.Range(1.0f, 1.5f);
            // TODO : 히트 시 넉백 효과로 공중으로 날린다고 하면 이 부분도 수정해야 함
            var impactY = 0;
            
            return new Vector2(impactX, impactY);
        }

        private void ApplyImpact()
        {
            ImpactDuration -= Time.deltaTime;

            if (!(ImpactDuration <= 0) || IsMoving)    return;

            ImpactDuration = 0;
            TargetSpeed = GetSpeed();
        }

        private void ApplySpeedBoost()
        {
            if (BoostTimer > 0)
            {
                BoostTimer -= Time.deltaTime;
                _currentSpeed += BoostSpeed;
            }
            else
            {
                BoostSpeed = 0;
            }
        }

        private void UpdatePosition()
        {
            var position = (Vector2)_targetTransform.position;

            // 수평 이동 처리
            if (IsMoving || WantToMove)
            {
                _currentSpeed = CalculateSpeed(_currentSpeed, TargetSpeed);
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
            float difference = targetSpeed - currentSpeed;
            if (Mathf.Abs(difference) > 0.0001f)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref _dampVelocity, DampTime);
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