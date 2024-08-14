using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillDeciderDistanceDTO), menuName = "State/Monster/" + nameof(MonsterSkillDeciderDTO) + "/" + nameof(MonsterSkillDeciderDistanceDTO))]
    public class MonsterSkillDeciderDistanceDTO : MonsterSkillDeciderDTO
    {
        public int distance;
        public Vector2 direction;
        public LayerMask targetLayer;

        public override IMonsterSkillDecider GetSkillDecider(IMonsterSkillAct targetSkill, Transform transform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterEnums, IMonsterFsmController fsmController)
        {
            return new MonsterSkillDeciderDistance(fsmController, targetSkill, distance, targetLayer, direction);
        }
    }
}