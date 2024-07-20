using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(HitStateDTO), menuName = "State/" + nameof(HitStateDTO))]
    public class HitStateDTO : BaseStateDTO
    {
        [Header("Death State Info")]
        [SerializeField] HitStateInfoDTO hitStateInfoDTO;
        public override IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa)
        {
            throw new System.NotImplementedException();
        }
    }

    [Serializable]
    public struct HitStateInfoDTO
    {

    }

    public struct HitStateInfo
    {

    }
}