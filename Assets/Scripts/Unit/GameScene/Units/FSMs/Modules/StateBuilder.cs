using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems;
using UnityEngine;

namespace Unit.GameScene.Units.FSMs.Modules
{
    public static class StateBuilder
    {
        public static StateMachine BuildMonsterStateMachine(MonsterStateMachineDTO monsterStateMachineDto, IMonsterFsmController fsmController, Dictionary<AnimationParameterEnums, int> animationParameter, Transform targetTransform, MonsterStatSystem monsterStatSystem)
        {
            return monsterStateMachineDto.Build(targetTransform, fsmController, animationParameter, monsterStatSystem);
        }

        public static StateMachine BuildCharacterStateMachine(CharacterStateMachineDto characterStateMachineDto, ICharacterFsmController fsmController, AnimatorSystem animatorSystem, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            return characterStateMachineDto.Build(fsmController, animatorSystem, animationParameter);
        }
    }
}