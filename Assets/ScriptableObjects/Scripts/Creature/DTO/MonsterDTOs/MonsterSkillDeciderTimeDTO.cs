using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillDeciderTimeDTO), menuName = "State/Monster/" + nameof(MonsterSkillDeciderDTO) + "/" + nameof(MonsterSkillDeciderTimeDTO))]
    public class MonsterSkillDeciderTimeDTO : MonsterSkillDeciderDTO
    {
        public float time;

        public override IMonsterSkillDecider GetSkillDecider(IMonsterSkillAct targetSkill, Transform transform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterEnums, IMonsterFsmController fsmController)
        {
            return new MonsterSkillDeciderTimer(fsmController, targetSkill, time);
        }
    }
}