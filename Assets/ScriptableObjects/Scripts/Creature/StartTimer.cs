using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(StartTimer), menuName = "State" + nameof(ActionData) + nameof(StartTimer))]
    public class StartTimer : ActionData {
        [SerializeField] private Timer targetTimer;

        public override IState OnAct(IState state) {
            targetTimer.StartTimer(state.StateMachine.Target);
            return state;
        }
    }
}