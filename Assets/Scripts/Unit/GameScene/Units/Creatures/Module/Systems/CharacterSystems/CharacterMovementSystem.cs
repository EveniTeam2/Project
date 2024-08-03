using Unit.GameScene.Units.Creatures.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems
{
    public class CharacterMovementSystem : MovementSystem
    {
        // TODO : 채이환 - 인코딩 UTF-8 통일해주세요.
        protected int BoostSpeed = 0;
        protected float BoostTimer = 0;

        private readonly CharacterStatSystem _characterStatSystem;

        public CharacterMovementSystem(CharacterStatSystem characterStatSystem, Transform characterTransform,
            float ground)
            : base(characterTransform, ground)
        {
            _characterStatSystem = characterStatSystem;
        }

        protected override int GetSpeed()
        {
            return _characterStatSystem.Speed + BoostSpeed;
        }

        public override void SetRun(bool isRun)
        {
            WantToMove = isRun;
            if (ImpactDuration > 0)
            {
                delayOrder = (new MovementOrder(SetRun, isRun));
                return;
            }
            delayOrder = null;
            TargetSpeed = isRun ? GetSpeed() : 0;
        }

        public override void SetBackward(bool isBack)
        {
            WantToBack = isBack;
            if (ImpactDuration > 0)
            {
                delayOrder = (new MovementOrder(SetBackward, isBack));
                return;
            }
            delayOrder = null;
            TargetSpeed = isBack ? -GetSpeed() : 0;
        }

        public void SetSpeedBoost(int boost, float duration)
        {
            BoostSpeed = boost;
            BoostTimer = duration;
            OnUpdate += ApplySpeedBoost;
        }

        private void ApplySpeedBoost()
        {
            if (BoostTimer > 0)
            {
                BoostTimer -= Time.deltaTime;
            }
            else
            {
                BoostSpeed = 0;
                OnUpdate -= ApplySpeedBoost;
            }
        }
    }

}