using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    public static class StateBuilder
    {
        public static StateMachine BuildState(StateMachineDTO stateDataDTO, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            return stateDataDTO.Build(transform, battleSystem, healthSystem, movementSystem, animator, animationParameter);
        }

        public static StateMachine MonsterBuildState(MonsterStateMachineDTO stateDataDTO, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, Dictionary<AnimationParameterEnums, int> animationParameter, MonsterEventPublisher eventPublisher)
        {
            return stateDataDTO.BuildMonster(transform, battleSystem, healthSystem, movementSystem, animator, animationParameter, eventPublisher);
        }
    }
}