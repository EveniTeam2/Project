using UnityEngine;

namespace Unit.Character {
    [CreateAssetMenu(fileName = nameof(CompositeActionData), menuName = "State/" + nameof(CompositeActionData))]
    public class CompositeActionData : ActionData {
        [SerializeField] ActionData actionData;
        [SerializeField] ActionData compositeActionData;

        public override void OnAct(BaseState state) {
            if (compositeActionData != null)
                compositeActionData.OnAct(state);
            actionData.OnAct(state);
        }
    }
}