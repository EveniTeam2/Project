using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM
{
    public static class StateBuilder
    {
        public static StateMachine BuildStateMachine(StateMachineDTO stateDataDTO, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, AnimatorEventReceiver animatorEventReceiver, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            return stateDataDTO.Build(transform, battleSystem, healthSystem, movementSystem, animatorEventReceiver, animationParameter);
        }
        public static StateMachine BuildStateMachine(MonsterStateMachineDTO stateDataDTO, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, AnimatorEventReceiver animatorEventReceiver, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            return stateDataDTO.BuildMonster(transform, battleSystem, healthSystem, movementSystem, animatorEventReceiver, animationParameter);
        }

        public static IState BuildState(BaseStateDTO data, Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, StateMachine stateMachine, AnimatorEventReceiver animatorEventReceiver, Dictionary<AnimationParameterEnums, int> animationParameter)
        {
            return data.Build(transform, battleSystem, healthSystem, movementSystem, animatorEventReceiver, stateMachine, animationParameter);
        }
    }
}