using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillDeciderCountDTO), menuName = "State/Monster/" + nameof(MonsterSkillDeciderDTO) + "/" + nameof(MonsterSkillDeciderCountDTO))]
    public class MonsterSkillDeciderCountDTO : MonsterSkillDeciderDTO
    {
        public int count;

        public override IMonsterSkillDecider GetSkillDecider(IMonsterSkillAct targetSkill, Transform transform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterEnums, IMonsterFsmController fsmController)
        {
            return new MonsterSkillDeciderCount(targetSkill, count);
        }
    }



}