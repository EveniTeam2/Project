using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public abstract class MonsterSkillDeciderDTO : ScriptableObject
    {
        public abstract IMonsterSkillDecider GetSkillDecider(IMonsterSkillAct targetSkill, Transform transform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterEnums, IMonsterFsmController fsmController);
    }
}