using Unit.GameScene.Stages.Creatures;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature {
    [CreateAssetMenu(fileName = nameof(CheckTimer), menuName = "State" + nameof(Condition) + nameof(CheckTimer))]
    public class CheckTimer : Condition {
        [SerializeField] private Timer targetTimer;

        public override bool CheckCondition(BaseCreature target) {
            return targetTimer.CheckCondition(target);
        }
    }
}