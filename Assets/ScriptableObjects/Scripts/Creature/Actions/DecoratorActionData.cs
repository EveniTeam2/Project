using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(DecoratorActionData), menuName = "State/Act/" + nameof(DecoratorActionData))]
    public class DecoratorActionData : ActionData
    {
        [SerializeField] private ActionData decoratorActionData;
        [SerializeField] private ActionData actionData;

        public override IStateAction GetStateAction()
        {
            var result = new StateActionDecoratorAction(decoratorActionData, actionData);
            return result;
        }
    }

    public class StateActionDecoratorAction : IStateAction {
        private IStateAction decoratorActionData;
        private IStateAction actionData;

        public StateActionDecoratorAction(ActionData decoratorActionData, ActionData actionData) {
            this.decoratorActionData = decoratorActionData.GetStateAction();
            this.actionData = actionData.GetStateAction();
        }

        public IState OnAct(IState state) {
            return actionData.OnAct(decoratorActionData.OnAct(state));
        }
    }
}