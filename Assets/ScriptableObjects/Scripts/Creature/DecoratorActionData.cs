using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(DecoratorActionData), menuName = "State/Act/" + nameof(DecoratorActionData))]
    public class DecoratorActionData : ActionData {
        [SerializeField] ActionData decoratorActionData;
        [SerializeField] ActionData actionData;

        public override IState OnAct(IState state) {
            return actionData.OnAct(decoratorActionData.OnAct(state));
        }
    }
}