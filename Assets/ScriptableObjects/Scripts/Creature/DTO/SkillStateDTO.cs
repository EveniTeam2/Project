using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(SkillStateDTO), menuName = "State/" + nameof(SkillStateDTO))]
    public class SkillStateDTO : BaseStateDTO
    {
        //[Header("Skill State Info")]
        //[SerializeField] protected SkillStateInfoDTO skillInfoDTO;

        public override IState Build(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa)
        {
            //return new SkillState(baseStateInfoDTO.GetInfo(anPa), skillInfoDTO.GetInfo(tr, ba, he, mo, an, st, anPa), st.TryChangeState, ba, an);
            return new SkillState(baseStateInfoDTO.GetInfo(anPa), st.TryChangeState);
        }
    }

    //[Serializable]
    //public struct SkillStateInfoDTO
    //{
    //    public AnimationParameterEnums skillParameter;
    //    public float skillValue;
    //    public LayerMask targetLayer;
    //    public Vector2 direction;
    //    public float distance;
    //    public float actTiming;
    //    public SkillActDTO skillAct;
    //    public SkillStateInfo GetInfo(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa)
    //    {
    //        return new SkillStateInfo(animationParameterEnums[skillParameter], skillValue, targetLayer, direction, distance, actTiming, skillAct.GetSkillAct(tr, ba, he, mo, an, st, anPa));
    //    }
    //}

    //public struct SkillStateInfo
    //{
    //    public int skillParameter;
    //    public float skillValue;
    //    public LayerMask targetLayer;
    //    public Vector2 direction;
    //    public float distance;
    //    public float actTiming;
    //    public ISkillAct skillAct;
    //    public SkillStateInfo(int skillParameter, float skillValue, LayerMask targetLayer, Vector2 direction, float distance, float actTiming, ISkillAct skillAct)
    //    {
    //        this.skillParameter = skillParameter;
    //        this.skillValue = skillValue;
    //        this.targetLayer = targetLayer;
    //        this.direction = direction;
    //        this.distance = distance;
    //        this.actTiming = actTiming;
    //        this.skillAct = skillAct;
    //    }
    //}
}