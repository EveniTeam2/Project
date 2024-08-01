using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Monster.Structs;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public abstract class MonsterBaseStateDto : ScriptableObject
    {
        [Header("Default State Info")]
        [SerializeField] protected MonsterBaseStateInfoDto monsterBaseStateInfoDto;
        
        public abstract IState BuildState(Transform targetTransform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterHash, IMonsterFsmController fsmController);
    }
}