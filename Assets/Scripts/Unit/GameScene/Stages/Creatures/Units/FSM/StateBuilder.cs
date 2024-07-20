using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    public static class StateBuilder
    {
        public static StateMachine BuildState(StateMachineDTO stateDataDTO, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            return stateDataDTO.Build(transform, battleSystem, healthSystem, movementSystem, animator, animationParameter);
        }

        public static IState BuildState(BaseStateDTO data, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            return data.Build(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine, animationParameter);
        }
    }
}