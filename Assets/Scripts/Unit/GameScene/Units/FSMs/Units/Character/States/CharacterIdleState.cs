using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;

namespace Unit.GameScene.Units.FSMs.Units.Character.States
{
    public class CharacterIdleState : CharacterBaseState
    {
        private readonly IdleStateInfo _idleStateInfo;

        public CharacterIdleState(CharacterBaseStateInfo characterBaseInfo, IdleStateInfo idleStateInfo, Func<StateType, bool> tryChangeState, AnimatorSystem animatorSystem, ICharacterFsmController fsmController)
            : base(characterBaseInfo, tryChangeState, animatorSystem, fsmController)
        {
            _idleStateInfo = idleStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            FsmController.SetBool(CharacterBaseStateInfo.StateParameter, true, null);
            OnFixedUpdate += CheckTargetAndRun;
        }
        public override void Exit()
        {
            base.Exit();
            FsmController.SetBool(CharacterBaseStateInfo.StateParameter, false, null);
            OnFixedUpdate -= CheckTargetAndRun;
        }

        protected virtual void CheckTargetAndRun()
        {
            if (FsmController.CheckEnemyInRange(_idleStateInfo.TargetLayer, _idleStateInfo.Direction, _idleStateInfo.Distance, out _)) return;
            
            OnFixedUpdate -= CheckTargetAndRun;
            TryChangeState.Invoke(StateType.Run);
        }
    }
}