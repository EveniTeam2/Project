using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(SetRun), menuName = "State/Act/" + nameof(SetRun))]
    public class SetRun : ActionData
    {
        [SerializeField] private bool isRun;

        public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine stateMachine)
        {
            var ret = new StateActionSetRun(isRun, movementSystem);
            return ret;
        }
    }

    public class StateActionSetRun : IStateAction
    {
        private bool _isRun;
        private readonly MovementSystem _movement;

        public StateActionSetRun(bool isRun, MovementSystem movmenet)
        {
            this._isRun = isRun;
            this._movement = movmenet;
        }

        public void OnAct(StateType stateName, int parameterHash)
        {
            _movement.SetRun(_isRun);
#if UNITY_EDITOR
            //Debug.Log($"IsMoving:{_movement.IsMoving} / WantToRun:{_isRun}");
#endif
        }
    }
}