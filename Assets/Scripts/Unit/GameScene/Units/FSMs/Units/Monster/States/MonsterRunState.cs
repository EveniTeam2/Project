using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using Unit.GameScene.Units.FSMs.Units.Monster.Structs;

namespace Unit.GameScene.Units.FSMs.Units.Monster.States
{
    public class MonsterRunState : MonsterBaseState
    {
        private readonly MonsterRunStateInfo _monsterRunStateInfo;

        public MonsterRunState(MonsterBaseStateInfo monsterBaseInfo,
            MonsterRunStateInfo monsterRunStateInfo,
            Func<StateType, bool> tryChangeState,
            IMonsterFsmController fsmController)
            : base(monsterBaseInfo, tryChangeState, fsmController)
        {
            _monsterRunStateInfo = monsterRunStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, true, null);
            FsmController.ToggleMovement(true);
            OnFixedUpdate += CheckTargetAndIdle;
        }
        
        public override void Exit()
        {
            base.Exit();
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, false, null);
            FsmController.ToggleMovement(false);
            OnFixedUpdate -= CheckTargetAndIdle;
        }
        
        private void CheckTargetAndIdle()
        {
            if (!FsmController.CheckEnemyInRange(_monsterRunStateInfo.TargetLayer, _monsterRunStateInfo.Direction, _monsterRunStateInfo.Distance, out _)) return;
            
            OnFixedUpdate -= CheckTargetAndIdle;
            TryChangeState.Invoke(StateType.Idle);
        }
    }
}