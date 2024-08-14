using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillDeciderCountAndDistanceDTO), menuName = "State/Monster/" + nameof(MonsterSkillDeciderDTO) + "/" + nameof(MonsterSkillDeciderCountAndDistanceDTO))]
    public class MonsterSkillDeciderCountAndDistanceDTO : MonsterSkillDeciderDTO
    {
        public int count;
        public float distance;
        public Vector2 direction;
        public LayerMask targetLayer;

        public override IMonsterSkillDecider GetSkillDecider(IMonsterSkillAct targetSkill, Transform transform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterEnums, IMonsterFsmController fsmController)
        {
            return new MonsterSkillDeciderCountAndDistance(fsmController, targetSkill, count, distance, targetLayer, direction);
        }
    }
}