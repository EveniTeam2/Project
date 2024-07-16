using Unit.GameScene.Stages.Creautres;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(CheckTimer), menuName = "State/" + nameof(Condition) + "/" + nameof(CheckTimer))]
    public class CheckTimer : Condition
    {
        [SerializeField] private Timer targetTimer;

        public override bool CheckCondition(BaseCreature target)
        {
            return targetTimer.CheckCondition(target);
        }

        public override Condition GetCopy()
        {
            var copy = CreateInstance<CheckTimer>();
            copy.targetTimer = targetTimer;
            return copy;
        }
    }
}