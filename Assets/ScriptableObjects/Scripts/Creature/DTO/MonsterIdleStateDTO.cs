﻿using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(MonsterIdleStateDTO), menuName = "State/Monster/" + nameof(MonsterIdleStateDTO))]
    public class MonsterIdleStateDTO : MonsterBaseStateDTO
    {
        [Header("Idle State Info")]
        [SerializeField] MonsterIdleStateInfoDTO mosnterIdleStateInfo;
        public override IState BuildMonster(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, StateMachine st, AnimatorEventReceiver animatorEventReceiver, Dictionary<AnimationParameterEnums, int> anPa)
        {
            return new MonsterIdleState(monsterBaseStateInfoDTO.GetInfo(anPa), mosnterIdleStateInfo.GetInfo(anPa), st.TryChangeState, animatorEventReceiver, ba);
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