using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(SetRun), menuName = "State/Act/" + nameof(SetRun))]
    public class SetRun : ActionData
    {
        [SerializeField] private bool isRun;

        public override IStateAction GetStateAction()
        {
            var ret = new StateActionSetRun(isRun);
            return ret;
        }
    }

    public class StateActionSetRun : IStateAction {
        private bool isRun;

        public StateActionSetRun(bool isRun) {
            this.isRun = isRun;
        }

        public IState OnAct(IState state) {
            state.StateMachine.Target.Movement.SetRun(isRun);
            return state;
        }
    }
}