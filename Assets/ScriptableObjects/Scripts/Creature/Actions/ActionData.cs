using System;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    public abstract class ActionData : ScriptableObject
    {
        public abstract IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine);
    }

    public interface IStateAction {
        void OnAct(StateType stateName, int parameterHash);
    }
}