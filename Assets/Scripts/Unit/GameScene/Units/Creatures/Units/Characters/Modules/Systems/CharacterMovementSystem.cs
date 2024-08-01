using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Interfaces.Movements;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules.Systems
{
    public class CharacterMovementSystem : MovementSystem, ICharacterMovement
    {
        private readonly CharacterStatSystem _characterStatSystem;
        // 속도 부스트
        protected float _boostSpeed;
        protected float _boostTimer;

        public CharacterMovementSystem(Transform characterTransform, CharacterStatSystem characterStatSystem, float ground)
            : base(characterTransform, ground)
        {
            _characterStatSystem = characterStatSystem;
        }

        public override void SetRun(bool isRun)
        {
            if (_impactDuration > 0)
                return;

            _wantToMove = isRun;
            _targetSpd = isRun ? _characterStatSystem.Speed : 0;
        }

        public override void SetBackward(bool isBack)
        {
            _wantToMove = isBack;
            if (_wantToMove)
                _targetSpd = -1 * _characterStatSystem.Speed;
            else
                _targetSpd = 0;
        }

        public void SetImpact(Vector2 impact, float duration)
        {
            throw new NotImplementedException();
        }

        public void SetSpeedBoost(float boost, float duration)
        {
            // TODO
        }

        private void SpeedBoost()
        {
            // TODO
        }
    }
}