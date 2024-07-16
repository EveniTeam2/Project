using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(DecoratorActionData), menuName = "State/Act/" + nameof(DecoratorActionData))]
    public class DecoratorActionData : ActionData
    {
        [SerializeField] private ActionData decoratorActionData;
        [SerializeField] private ActionData actionData;

        public override IState OnAct(IState state)
        {
            return actionData.OnAct(decoratorActionData.OnAct(state));
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<DecoratorActionData>();
            copy.decoratorActionData = decoratorActionData.GetCopy();
            copy.actionData = actionData.GetCopy();
            return copy;
        }
    }
}