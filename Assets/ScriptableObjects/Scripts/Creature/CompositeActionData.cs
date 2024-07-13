using Unit.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CompositeActionData), menuName = "State/" + nameof(CompositeActionData))]
    public class CompositeActionData : ActionData {
        [SerializeField] ActionData actionData;
        [SerializeField] ActionData compositeActionData;

        public override void OnAct(IState state) {
            if (compositeActionData != null)
                compositeActionData.OnAct(state);
            actionData.OnAct(state);
        }
    }
}