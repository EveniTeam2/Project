using UnityEngine;
using Unit.Stages.Creatures.Characters.Unit.Character;
using Unit.Stages.Creatures.Monsters;
using System;
using Unit.Stages.Creatures.Interfaces;

namespace Unit.Stages.Creatures {
    public class MovementSystem : IRunnable {

        int IRunnable.Speed => _speed;
        bool IRunnable.IsRun => _currentSpd > 0;

        private int _speed => GetSpeed.Invoke();
        Func<int> GetSpeed;

        private bool _isRun;
        private bool _isJump;
        private BaseCreature _character;

        private float _currentSpd;
        private float _targetSpd;
        private float _dampTime = 0.3f;
        private float _dampVel;

        private float _currentJmp;

        public MovementSystem(BaseCreature character, Stat<CharacterStat> stats) {
            _character = character;
            GetSpeed = () => stats.Current.Speed;
        }

        public MovementSystem(BaseCreature character, Stat<MonsterStat> stats) {
            _character = character;
            GetSpeed = () => stats.Current.Speed;
        }

        void IRunnable.SetRun(bool isRun) {
            _isRun = isRun;
            if (isRun)
                _targetSpd = _speed;
            else
                _targetSpd = 0;
        }

        private float CalculateSpeed(float currentSpd, float targetSpd) {
            var value = (currentSpd - targetSpd);
            if (value * value > 0.0001f) {
                currentSpd = Mathf.SmoothDamp(currentSpd, targetSpd, ref _dampVel, _dampTime);
            }
            else {
                currentSpd = targetSpd;
            }
            return currentSpd;
        }

        void IRunnable.Update() {
            if ((this as IRunnable).IsRun || _isRun) {
                _currentSpd = CalculateSpeed(_currentSpd, _targetSpd);

                float dist = _currentSpd * Time.deltaTime;
                Vector2 pos = _character.transform.position;

                // TODO rigidbody 쓸거면 바꿔야 됨
                _character.transform.position = new Vector2(pos.x + dist, pos.y);
            }
        }

        void IRunnable.FixedUpdate() {
            if ((this as IRunnable).IsRun || _isRun) {
                _currentSpd = CalculateSpeed(_currentSpd, _targetSpd);
            }
        }
    }
}