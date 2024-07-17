using System;
using System.Collections;
using Unit.GameScene.Stages.Creatures.Characters.Unit.Character;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Monsters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.FSM.ActOnInput
{
    public class MovementSystem : IRunnable
    {
        private readonly Transform _targetTransform;
        private const float _gravity = -20f;
        private float _currentYSpd;

        private float _currentSpd;
        private readonly float _dampTime = 0.3f;
        private float _dampVel;
        private bool _isJump;

        private bool _isRun;
        private float _targetSpd;
        private readonly Func<int> GetSpeed;

        private float _forward;
        private float _targetDirection;

        private float _boostTimer;
        private float _boostSpeed;
        
        public bool IsJump => _targetTransform.position.y > GroundYPosition;
        public float GroundYPosition { get; protected set; }
        private int _speed => GetSpeed.Invoke();
        public int Speed => _speed;
        public bool IsRun => _currentSpd * _currentSpd > 0;

        public MovementSystem(Transform targetTransform, Stat<CharacterStat> stats)
        {
            _targetTransform = targetTransform;
            GetSpeed = () => stats.Current.Speed;
            _forward = 1f;
        }

        public MovementSystem(Transform targetTransform, Stat<MonsterStat> stats)
        {
            _targetTransform = targetTransform;
            GetSpeed = () => stats.Current.Speed;
            _forward = -1f;
        }

        public void SetSpeedBoost(float boost, float duration) {
            // TODO

        }

        private void SpeedBoost() {

        }

        public void SetRun(bool isRun)
        {
            _targetDirection = _forward;
            _isRun = isRun;
            if (isRun)
                _targetSpd = _targetDirection * _speed;
            else
                _targetSpd = 0;
        }

        public void Update()
        {
            Vector2 pos = _targetTransform.position;
            if ((this as IRunnable).IsRun || _isRun)
            {
                _currentSpd = CalculateSpeed(_currentSpd, _targetSpd);
                pos.x += _currentSpd * Time.deltaTime;
            }

            if (IsJump || _isJump)
            {
                pos.y += _currentYSpd * Time.deltaTime;
                if (pos.y < GroundYPosition)
                    pos.y = GroundYPosition;
                _isJump = false;
            }

            _targetTransform.position = pos;
        }

        public void FixedUpdate()
        {
            if ((this as IRunnable).IsRun || _isRun) _currentSpd = CalculateSpeed(_currentSpd, _targetSpd);
            JumpFixedUpdate();
        }

        public void SetJump(float power)
        {
            _isJump = true;
            _currentYSpd = power;
        }

        private float CalculateSpeed(float currentSpd, float targetSpd)
        {
            var value = currentSpd - targetSpd;
            if (value * value > 0.0001f)
                currentSpd = Mathf.SmoothDamp(currentSpd, targetSpd, ref _dampVel, _dampTime);
            else
                currentSpd = targetSpd;
            return currentSpd;
        }

        private void JumpFixedUpdate()
        {
            _currentYSpd += _gravity * Time.fixedDeltaTime;
        }

        public void SetGroundPosition(float groundYPosition)
        {
            GroundYPosition = groundYPosition;
        }

        public void SetBackStep(bool isBack) {
            _targetDirection = _forward * -1;
            _isRun = isBack;
            if (_isRun)
                _targetSpd = _targetDirection * _speed;
            else
                _targetSpd = 0;
        }
    }
}