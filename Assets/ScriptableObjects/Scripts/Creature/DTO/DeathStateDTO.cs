using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(DeathStateDTO), menuName = "State/" + nameof(DeathStateDTO))]
    public class DeathStateDTO : BaseStateDTO
    {
        //[Header("Death State Info")]
        //[SerializeField] DeathStateInfoDTO deathStateInfoDTO;
        public override IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, AnimatorEventReceiver animatorEventReceiver, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa)
        {
            return new DeathState(baseStateInfoDTO.GetInfo(anPa), st.TryChangeState, animatorEventReceiver);
        }
    }

    //

    //[Serializable]
    //public struct DeathStateInfoDTO
    //{

    //}

    //public struct DeathStateInfo
    //{

    //}
}