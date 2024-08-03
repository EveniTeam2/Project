using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public abstract class MonsterSkillActDTO : ScriptableObject
    {
        public abstract IMonsterSkillAct GetSkillAct(Transform transform, IMonsterFsmController fsmController, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterEnums);
    }

}