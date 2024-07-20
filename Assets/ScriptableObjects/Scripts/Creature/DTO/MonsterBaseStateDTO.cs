using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public abstract class MonsterBaseStateDTO : ScriptableObject
    {
        [Header("Monster State Info")]
        [SerializeField] protected MonsterBaseStateinfoDTO monsterBaseStateInfoDTO;
        public abstract IState BuildMonster(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa, MonsterEventPublisher eventPublisher);
    }

    public class MonsterBaseStateinfoDTO
    {
        public BaseStateInfoDTO BaseStateInfoDTO;
        public StateType _defaultExitState;

        public MonsterBaseStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new MonsterBaseStateInfo(_defaultExitState);
        }
        public BaseStateInfo GetBaseInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return BaseStateInfoDTO.GetInfo(animationParameterEnums);
        }
    }

    public struct MonsterBaseStateInfo
    {
        public StateType _defaultExitState;

        public MonsterBaseStateInfo(StateType defaultExitState)
        {
            _defaultExitState = defaultExitState;
        }
    }
}