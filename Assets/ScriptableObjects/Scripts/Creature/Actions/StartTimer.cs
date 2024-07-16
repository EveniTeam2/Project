using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(StartTimer), menuName = "State/" + nameof(ActionData) + "/" + nameof(StartTimer))]
    public class StartTimer : ActionData
    {
        [SerializeField] private Timer targetTimer;

        public override IState OnAct(IState state)
        {
            targetTimer.StartTimer(state.StateMachine.Target);
            return state;
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<StartTimer>();
            copy.targetTimer = targetTimer;
            return copy;
        }
    }
}