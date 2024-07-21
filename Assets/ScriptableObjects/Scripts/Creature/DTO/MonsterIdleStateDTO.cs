using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(IdleStateDTO), menuName = "State/" + nameof(IdleStateDTO))]
    public class MonsterIdleStateDTO : MonsterBaseStateDTO
    {
        [Header("Idle State Info")]
        [SerializeField] MonsterIdleStateInfoDTO mosnterIdleStateInfo;
        public override IState BuildMonster(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa, MonsterEventPublisher eventPublisher)
        {
            return new MonsterIdleState(monsterBaseStateInfoDTO.GetInfo(anPa), mosnterIdleStateInfo.GetInfo(anPa), st.TryChangeState, an, ba, eventPublisher);
        }
    }

    [Serializable]
    public struct MonsterIdleStateInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public MonsterIdleStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new MonsterIdleStateInfo(targetLayer, direction, distance);
        }
    }

    public struct MonsterIdleStateInfo
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public MonsterIdleStateInfo(LayerMask targetLayer, Vector2 direction, float distance)
        {
            this.targetLayer = targetLayer;
            this.direction = direction;
            this.distance = distance;
        }
    }
}