using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(HitStateDTO), menuName = "State/" + nameof(HitStateDTO))]
    public class HitStateDTO : BaseStateDTO
    {
        [Header("Hit State Info")]
        [SerializeField] HitStateInfoDTO hitStateInfoDTO;
        public override IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, AnimatorEventReceiver animatorEventReceiver, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa)
        {
            return new HitState(baseStateInfoDTO.GetInfo(anPa), hitStateInfoDTO.GetInfo(), st.TryChangeState, animatorEventReceiver);
        }
    }

    [Serializable]
    public struct HitStateInfoDTO
    {
        public StateType _defaultStateType;
        public HitStateInfo GetInfo()
        {
            return new HitStateInfo(_defaultStateType);
        }
    }

    public struct HitStateInfo
    {
        public StateType _defaultStateType;

        public HitStateInfo(StateType defaultStateType)
        {
            _defaultStateType = defaultStateType;
        }
    }
}