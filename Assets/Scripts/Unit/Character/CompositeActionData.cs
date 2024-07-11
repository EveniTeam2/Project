using UnityEngine;

namespace Unit.Character {
    [CreateAssetMenu(fileName = nameof(CompositeActionData), menuName = "State/" + nameof(CompositeActionData))]
    public class CompositeActionData : ActionData {
        [SerializeField] ActionData actionData;
        [SerializeField] CompositeActionData compositeActionData;

        public override void OnAct() {
            if (compositeActionData != null)
                compositeActionData.actionData.OnAct();
            actionData.OnAct();
        }
    }
}