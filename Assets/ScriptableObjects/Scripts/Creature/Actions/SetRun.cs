using Unit.GameScene.Stages.Creatures.Interfaces;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions {
    [CreateAssetMenu(fileName = nameof(SetRun), menuName = "State/Act/" + nameof(SetRun))]
    public class SetRun : ActionData {
        [SerializeField] bool isRun;
        public override IState OnAct(IState state) {
            state.StateMachine.Target.Movement.SetRun(isRun);
            return state;
        }
    }

    
}