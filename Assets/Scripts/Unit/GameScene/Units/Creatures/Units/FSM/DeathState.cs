using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.FSM
{
    public class DeathState : BaseState
    {
        //private readonly DeathStateInfo _deathInfo;

        public DeathState(BaseStateInfo baseStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver)
            : base(baseStateInfo, tryChangeState, animatorEventReceiver)
        {
            //_deathInfo = deathInfo;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorEventReceiver.SetTrigger(_baseStateInfo.stateParameter, null);
        }
        public override void Exit()
        {
            base.Exit();
        }
    }

    public class MonsterDeathState : MonsterBaseState
    {
        //private readonly MonsterDeathStateInfo _deathInfo;

        public MonsterDeathState(MonsterBaseStateInfo monsterBaseStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver)
            : base(monsterBaseStateInfo, tryChangeState, animatorEventReceiver)
        {
            //_deathInfo = deathInfo;
        }

        public override void Enter()
        {
            base.Enter();
            animatorEventReceiver.SetTrigger(_monsterBaseStateInfo.stateParameter, ChangeToDefaultState);
            Debug.Log("Monster is dead start.");
        }
        public override void Exit()
        {
            base.Exit();
            Debug.Log("Monster is dead end.");
        }
    }
}