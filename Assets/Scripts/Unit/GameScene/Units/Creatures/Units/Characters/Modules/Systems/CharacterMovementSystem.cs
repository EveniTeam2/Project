using Unit.GameScene.Units.Creatures.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules.Systems
{
    public class CharacterMovementSystem : MovementSystem
    {
        private readonly CharacterStatSystem _characterStatSystem;

        public CharacterMovementSystem(CharacterStatSystem characterStatSystem, Transform characterTransform,
            float ground)
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
            if (ImpactDuration > 0)
                return;

            WantToMove = isRun;
            TargetSpeed = isRun ? GetSpeed() : 0;
        }

        public override void SetBackward(bool isBack)
        {
            if (ImpactDuration > 0)
                return;

            WantToMove = isBack;
            TargetSpeed = isBack ? -GetSpeed() : 0;
        }

        public void SetSpeedBoost(float boost, float duration)
        {
            BoostSpeed = boost;
            BoostTimer = duration;
        }
    }
}