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

    [CreateAssetMenu(fileName = nameof(SkillStateDTO), menuName = "State/" + nameof(SkillStateDTO))]
    public class SkillStateDTO : BaseStateDTO
    {
        //[Header("Skill State Info")]
        //[SerializeField] protected SkillStateInfoDTO skillInfoDTO;

        public override IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa, MonsterEventPublisher eventPublisher)
        {
            //return new SkillState(baseStateInfoDTO.GetInfo(anPa), skillInfoDTO.GetInfo(tr, ba, he, mo, an, st, anPa), st.TryChangeState, ba, an);
            return new SkillState(baseStateInfoDTO.GetInfo(anPa), st.TryChangeState);
        }
    }

}