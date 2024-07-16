using System;
using Unit.GameScene.Stages.Creatures.Characters.Unit.Character;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Monsters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.FSM.ActOnInput
{
    public class MovementSystem : IRunnable
    {
        private const float _gravity = -20f;
        private readonly BaseCreature _character;

        private float _currentSpd;

        private float _currentYSpd;
        private readonly float _dampTime = 0.3f;
        private float _dampVel;
        private bool _isJump;

        private bool _isRun;
        private float _targetSpd;
        private readonly Func<int> GetSpeed;

        public MovementSystem(BaseCreature character, Stat<CharacterStat> stats)
        {
            _character = character;
            GetSpeed = () => stats.Current.Speed;
        }

        public MovementSystem(BaseCreature character, Stat<MonsterStat> stats)
        {
            _character = character;
            GetSpeed = () => stats.Current.Speed;
        }

        public bool IsJump => _character.transform.position.y > GroundYPosition;
        public float GroundYPosition { get; protected set; }

        private int _speed => GetSpeed.Invoke();

        public int Speed => _speed;
        public bool IsRun => _currentSpd > 0;

        public void SetRun(bool isRun)
        {
            _isRun = isRun;
            if (isRun)
                _targetSpd = _speed;
            else
                _targetSpd = 0;
        }

        public void Update()
        {
            Vector2 pos = _character.transform.position;
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

            _character.transform.position = pos;
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
    }
}