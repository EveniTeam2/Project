using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Module;
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
            return new IdleState(baseStateInfoDTO.GetInfo(anPa), idleInfoDTO.GetInfo(anPa), st.TryChangeState, animatorEventReceiver, ba);
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