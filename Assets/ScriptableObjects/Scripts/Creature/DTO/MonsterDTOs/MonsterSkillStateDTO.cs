using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Monster.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillStateDTO), menuName = "State/Monster/" + nameof(MonsterSkillStateDTO))]
    public class MonsterSkillStateDTO : MonsterBaseStateDto
    {
        [Header("Skill State Info")]
        [SerializeField] protected MonsterSkillStateInfoDTO skillInfoDto;
        
        public override IState BuildState(
            Transform targetTransform, 
            StateMachine stateMachine,
            Dictionary<AnimationParameterEnums, int> animationParameterHash,
            IMonsterFsmController fsmController)
        {
            return new MonsterSkillState(
                monsterBaseStateInfoDto.GetInfo(animationParameterHash),
                skillInfoDto.GetInfo(targetTransform, stateMachine, animationParameterHash, fsmController),
                stateMachine.TryChangeState,
                fsmController);
        }
    }

    [Serializable]
    public struct MonsterSkillStateInfoDTO
    {
        public AnimationParameterEnums skillParameter;
        public int skillValue;
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;
        public MonsterSkillActDTO skillAct;
        
        public MonsterSkillStateInfo GetInfo(
            Transform transform,
            StateMachine stateMachine,
            Dictionary<AnimationParameterEnums, int> animationParameterEnums,
            IMonsterFsmController fsmController)
        {
            return new MonsterSkillStateInfo(animationParameterEnums[skillParameter], skillValue, targetLayer, direction, distance, skillAct.GetSkillAct(transform, fsmController, stateMachine, animationParameterEnums));
        }
    }

    public struct MonsterSkillStateInfo
    {
        public readonly int SkillParameter;
        public readonly int SkillValue;
        public readonly float Distance;
        public readonly IMonsterSkillAct SkillAct;
        
        public LayerMask TargetLayer;
        public Vector2 Direction;
        
        public MonsterSkillStateInfo(int skillParameter, int skillValue, LayerMask targetLayer, Vector2 direction, float distance, IMonsterSkillAct skillAct)
        {
            SkillParameter = skillParameter;
            SkillValue = skillValue;
            TargetLayer = targetLayer;
            Direction = direction;
            Distance = distance;
            SkillAct = skillAct;
        }
    }
}