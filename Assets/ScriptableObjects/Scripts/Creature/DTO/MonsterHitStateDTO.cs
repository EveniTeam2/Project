using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    //[Serializable]
    //public struct HitStateInfoDTO
    //{

    //}

    //public struct HitStateInfo
    //{

    //}

    [CreateAssetMenu(fileName = nameof(MonsterHitStateDTO), menuName = "State/Monster" + nameof(MonsterHitStateDTO))]
    public class MonsterHitStateDTO : MonsterBaseStateDTO
    {
        //[Header("Hit State Info")]
        //[SerializeField] HitStateInfoDTO hitStateInfoDTO;
        public override IState BuildMonster(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa, MonsterEventPublisher monsterEventPublisher)
        {
            return new MonsterHitState(monsterBaseStateInfoDTO.GetInfo(anPa), st.TryChangeState, monsterEventPublisher);
        }
    }

    //[Serializable]
    //public struct HitStateInfoDTO
    //{

    //}

    //public struct HitStateInfo
    //{

    //}
}