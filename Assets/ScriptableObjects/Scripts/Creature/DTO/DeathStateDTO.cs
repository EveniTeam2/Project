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
    [CreateAssetMenu(fileName = nameof(DeathStateDTO), menuName = "State/" + nameof(DeathStateDTO))]
    public class DeathStateDTO : BaseStateDTO
    {
        //[Header("Death State Info")]
        //[SerializeField] DeathStateInfoDTO deathStateInfoDTO;
        public override IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa, MonsterEventPublisher eventPublisher)
        {
            return new DeathState(baseStateInfoDTO.GetInfo(anPa), st.TryChangeState, an);
        }
    }

    //[Serializable]
    //public struct DeathStateInfoDTO
    //{

    //}

    //public struct DeathStateInfo
    //{

    //}
}