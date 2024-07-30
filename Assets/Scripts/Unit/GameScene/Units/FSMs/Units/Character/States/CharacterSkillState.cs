using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.Animations;

namespace Unit.GameScene.Units.FSMs.Units.Character.States
{

    public class CharacterSkillState : CharacterBaseState
    {
        public CharacterSkillState(CharacterBaseStateInfo characterBaseInfo, Func<StateType, bool> tryChangeState, AnimatorSystem animatorSystem, ICharacterFsmController fsmController)
            : base(characterBaseInfo, tryChangeState, animatorSystem, fsmController) { }
        
        public override void Exit()
        {
            base.Exit();
            ClearEvent(EStateEventType.Enter);
            ClearEvent(EStateEventType.Exit);
            ClearEvent(EStateEventType.Update);
            ClearEvent(EStateEventType.FixedUpdate);
        }
    }
}