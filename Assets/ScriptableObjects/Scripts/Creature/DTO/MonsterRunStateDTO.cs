using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(MonsterRunStateDTO), menuName = "State/Monster/" + nameof(MonsterRunStateDTO))]
    public class MonsterRunStateDTO : MonsterBaseStateDTO
    {
        [Header("Run State Info")]
        [SerializeField] MonsterRunStateInfoDTO monsterRunStateInfoDTO;
        public override IState BuildMonster(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, StateMachine st, AnimatorEventReceiver animatorEventReceiver, Dictionary<AnimationParameterEnums, int> anPa)
        {
            return new MonsterRunState(monsterBaseStateInfoDTO.GetInfo(anPa), monsterRunStateInfoDTO.GetInfo(), st.TryChangeState, ba, mo, animatorEventReceiver);
        }
    }

    [Serializable]
    public struct MonsterRunStateInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public MonsterRunStateInfo GetInfo()
        {
            return new MonsterRunStateInfo(distance, direction, targetLayer);
        }
    }

    public struct MonsterRunStateInfo
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public MonsterRunStateInfo(float distance, Vector2 direction, LayerMask targetLayer) : this()
        {
            this.distance = distance;
            this.direction = direction;
            this.targetLayer = targetLayer;
        }
    }
}