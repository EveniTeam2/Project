using ScriptableObjects.Scripts.Creature.Actions;
using ScriptableObjects.Scripts.Creature.Conditions;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO {
    [CreateAssetMenu(fileName = nameof(ActOnInput), menuName = "State/"+ nameof(FullStateData) + "/" + nameof(FullStateActOne) )]
    public class FullStateActOne : FullStateData {
        [SerializeField] ActionData _act;
        [SerializeField] Condition _con;
        public override FullState GetFullState(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine sm) {
            return new ActOneFixedUpdate(_act.GetStateAction(tr, ba, he, mo, an, sm), _con.GetStateCondition(tr, ba, he, mo, an));
        }
    }
}