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
    public class MonsterIdleState : MonsterBaseState
    {
        private readonly MonsterIdleStateInfo _monsterIdleStateInfo;

        public MonsterIdleState(MonsterBaseStateInfo baseInfo, MonsterIdleStateInfo monsterIdleStateInfo, Func<StateType, bool> tryChangeState, IMonsterFsmController fsmController)
            : base(baseInfo, tryChangeState, fsmController)
        {
            _monsterIdleStateInfo = monsterIdleStateInfo;
        }

        public override void Enter()
        {
            base.Enter();
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, true, null);
            OnFixedUpdate += CheckTargetAndRun;
        }
        public override void Exit()
        {
            base.Exit();
            FsmController.SetBool(MonsterBaseStateInfo.StateParameter, false, null);
            OnFixedUpdate -= CheckTargetAndRun;
        }

        protected virtual void CheckTargetAndRun()
        {
            if (!FsmController.CheckEnemyInRange(_monsterIdleStateInfo.TargetLayer, _monsterIdleStateInfo.Direction, _monsterIdleStateInfo.Distance, out var targets))
            {
                OnFixedUpdate -= CheckTargetAndRun;
                TryChangeState.Invoke(StateType.Run);
            }
            else
            {
                if (FsmController.IsReadyForAttack())
                {
                    TryChangeState.Invoke(StateType.Skill);
                }
            }
        }
    }
}