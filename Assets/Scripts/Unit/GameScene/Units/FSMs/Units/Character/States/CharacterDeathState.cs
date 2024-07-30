using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.Animations;

namespace Unit.GameScene.Units.FSMs.Units.Character.States
{
    public class CharacterDeathState : CharacterBaseState
    {
        private readonly DeathStateInfo _deathInfo;

        public CharacterDeathState(CharacterBaseStateInfo characterBaseStateInfo, DeathStateInfo deathInfo, Func<StateType, bool> tryChangeState, AnimatorSystem animatorSystem, ICharacterFsmController fsmController)
            : base(characterBaseStateInfo, tryChangeState, animatorSystem, fsmController)
        {
            _deathInfo = deathInfo;
        }

        public override void Enter()
        {
            base.Enter();
            FsmController.SetTrigger(CharacterBaseStateInfo.StateParameter, null);
        }
    }
}