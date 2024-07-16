using Unit.GameScene.Stages.Creautres.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(SetRun), menuName = "State/Act/" + nameof(SetRun))]
    public class SetRun : ActionData
    {
        [SerializeField] private bool isRun;

        public override IState OnAct(IState state)
        {
            state.StateMachine.Target.Movement.SetRun(isRun);
            return state;
        }

        public override ActionData GetCopy()
        {
            var copy = CreateInstance<SetRun>();
            copy.isRun = isRun;
            return copy;
        }
    }
}