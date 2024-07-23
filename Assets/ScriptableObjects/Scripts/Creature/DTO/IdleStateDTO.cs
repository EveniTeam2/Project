using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(IdleStateDTO), menuName = "State/" + nameof(IdleStateDTO))]
    public class IdleStateDTO : BaseStateDTO
    {
        [Header("Idle State Info")]
        [SerializeField] IdleStateInfoDTO idleInfoDTO;
        public override IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, AnimatorEventReceiver animatorEventReceiver, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa)
        {
            return new IdleState(baseStateInfoDTO.GetInfo(anPa), idleInfoDTO.GetInfo(anPa), st.TryChangeState, ba, animatorEventReceiver);
        }
    }

    [Serializable]
    public struct IdleStateInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public IdleStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new IdleStateInfo(targetLayer, direction, distance);
        }
    }

    public struct IdleStateInfo
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public IdleStateInfo(LayerMask targetLayer, Vector2 direction, float distance) : this()
        {
            this.targetLayer = targetLayer;
            this.direction = direction;
            this.distance = distance;
        }
    }
}