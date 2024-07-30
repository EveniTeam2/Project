using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.Animations;

namespace Unit.GameScene.Units.FSMs.Units.Character.States
{
    public class CharacterRunState : CharacterBaseState
    {
        private readonly RunStateInfo _runStateInfo;

        public CharacterRunState(CharacterBaseStateInfo characterBaseInfo, RunStateInfo runStateInfo, Func<StateType, bool> tryChangeState, AnimatorSystem animatorSystem, ICharacterFsmController fsmController) : base(characterBaseInfo, tryChangeState, animatorSystem, fsmController)
        {
            _runStateInfo = runStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            FsmController.SetBool(CharacterBaseStateInfo.StateParameter, true, null);
            FsmController.ToggleMovement(true);
            OnFixedUpdate += CheckTargetAndIdle;
        }
        
        public override void Exit()
        {
            base.Exit();
            FsmController.SetBool(CharacterBaseStateInfo.StateParameter, false, null);
            FsmController.ToggleMovement(false);
            OnFixedUpdate -= CheckTargetAndIdle;
        }
        
        private void CheckTargetAndIdle()
        {
            if (FsmController.CheckEnemyInRange(_runStateInfo.TargetLayer, _runStateInfo.Direction, _runStateInfo.Distance, out _))
            {
                OnFixedUpdate -= CheckTargetAndIdle;
                TryChangeState.Invoke(StateType.Idle);
            }
        }
    }
}