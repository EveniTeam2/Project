using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;

namespace Unit.GameScene.Units.FSMs.Units.Character.States
{
    public class CharacterHitState : CharacterBaseState
    {
        private readonly HitStateInfo _hitStateInfo;

        public CharacterHitState(CharacterBaseStateInfo characterBaseStateInfo, HitStateInfo hitStateInfo, Func<StateType, bool> tryChangeState, AnimatorSystem animatorSystem, ICharacterFsmController fsmController)
            : base(characterBaseStateInfo, tryChangeState, animatorSystem, fsmController)
        {
            _hitStateInfo = hitStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            
            FsmController.SetTrigger(CharacterBaseStateInfo.StateParameter, ChangeToDefaultState);
        }

        private void ChangeToDefaultState()
        {
            TryChangeState.Invoke(_hitStateInfo.DefaultStateType);
        }
    }
}