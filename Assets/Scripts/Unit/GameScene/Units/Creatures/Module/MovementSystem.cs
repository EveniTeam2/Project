using System;
using Unit.GameScene.Stages.Creatures.Units.Monsters.Modules;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Module
{
    public abstract class MovementSystem
    {
        // Y movement 관련
        protected const float _gravity = -20f;

        // 속도 댐핑 변수
        protected readonly float _dampTime = 0.3f;
        protected IMovementStat _stats;
        protected readonly Transform _targetTransform;

        // X movement 관련
        protected float _currentSpd;
        protected float _dampVel;
        protected float _groundYPosition;
        protected float _targetSpd;
        protected bool _wantToMove;

        public MovementSystem(Transform targetTransform, IMovementStat stats, float ground)
        {
            _targetTransform = targetTransform;
            _groundYPosition = ground;
            _stats = stats;
        }

        // 움직임 판단
        public bool IsInAir => _targetTransform.position.y > _groundYPosition;
        public bool IsMoving => _currentSpd * _currentSpd > 0;

        public virtual void Update()
        {
            Vector2 pos = _targetTransform.position;

            if (IsMoving || _wantToMove)
            {
                _currentSpd = CalculateSpeed(_currentSpd, _targetSpd);
                pos.x += _currentSpd * Time.deltaTime;
            }

            _targetTransform.position = pos;
#if UNITY_EDITOR
            //Debug.Log($"현재 속도:{_currentSpd}/{_targetSpd}");
#endif
        }

        public virtual void FixedUpdate()
        {
            if (IsMoving || _wantToMove)
                _currentSpd = CalculateSpeed(_currentSpd, _targetSpd);
        }

        public abstract void SetRun(bool isRun);
        public abstract void SetBackward(bool isBackward);

        public virtual void SetGroundPosition(float groundYPosition)
        {
            _groundYPosition = groundYPosition;
        }

        public abstract void Jump(float power);

        protected virtual float CalculateSpeed(float currentSpd, float targetSpd)
        {
            var value = currentSpd - targetSpd;
            if (value * value > 0.0001f)
                currentSpd = Mathf.SmoothDamp(currentSpd, targetSpd, ref _dampVel, _dampTime);
            else
                currentSpd = targetSpd;
            return currentSpd;
        }

        internal void SpawnInit(MonsterMovementStat monsterMovementStat)
        {
            _stats = monsterMovementStat;
        }
    }

    public interface IMovementStat
    {
        int GetSpeed();
    }
}