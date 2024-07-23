using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(MonsterDeathStateDTO), menuName = "State/Monster/" + nameof(MonsterDeathStateDTO))]
    public class MonsterDeathStateDTO : MonsterBaseStateDTO
    {
        //[Header("Death State Info")]
        //[SerializeField] DeathStateInfoDTO deathStateInfoDTO;
        public override IState BuildMonster(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, StateMachine st, AnimatorEventReceiver animatorEventReceiver, Dictionary<AnimationParameterEnums, int> anPa)
        {
            return new MonsterDeathState(monsterBaseStateInfoDTO.GetInfo(anPa), st.TryChangeState, animatorEventReceiver);
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