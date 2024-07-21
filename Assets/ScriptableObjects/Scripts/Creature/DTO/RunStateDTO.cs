using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(RunStateDTO), menuName = "State/" + nameof(RunStateDTO))]
    public class RunStateDTO : BaseStateDTO
    {
        [Header("Default State Info")]
        [SerializeField] RunStateInfoDTO runStateInfoDTO;
        public override IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa)
        {
            return new RunState(baseStateInfoDTO.GetInfo(anPa), runStateInfoDTO.GetInfo(), st.TryChangeState, ba, mo, an);
        }
    }

    [Serializable]
    public struct RunStateInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public RunStateInfo GetInfo()
        {
            return new RunStateInfo(distance, direction, targetLayer);
        }
    }

    public struct RunStateInfo
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public RunStateInfo(float distance, Vector2 direction, LayerMask targetLayer) : this()
        {
            this.distance = distance;
            this.direction = direction;
            this.targetLayer = targetLayer;
        }
    }
}