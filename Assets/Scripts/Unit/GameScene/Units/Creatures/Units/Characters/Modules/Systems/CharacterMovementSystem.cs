using Unit.GameScene.Units.Creatures.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules.Systems
{
    public class CharacterMovementSystem : MovementSystem
    {
        private readonly CharacterStatSystem _characterStatSystem;

        public CharacterMovementSystem(CharacterStatSystem characterStatSystem, Transform characterTransform, float ground)
            : base(characterTransform, ground)
        {
            _characterStatSystem = characterStatSystem;
        }

        protected override int GetSpeed()
        {
            return _characterStatSystem.Speed;
        }

        public override void SetRun(bool isRun)
        {
            if (_impactDuration > 0)
                return;

            _wantToMove = isRun;
            _targetSpeed = isRun ? GetSpeed() / 2 : 0;
        }

        public override void SetBackward(bool isBack)
        {
            if (_impactDuration > 0)
                return;

            _wantToMove = isBack;
            _targetSpeed = isBack ? -GetSpeed() : 0;
        }

        public void SetSpeedBoost(float boost, float duration)
        {
            _boostSpeed = boost;
            _boostTimer = duration;
        }
    }
}