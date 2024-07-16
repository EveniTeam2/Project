using Unit.GameScene.Stages.Creautres;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(CheckAnimationTime), menuName = "State/Condition/" + nameof(CheckAnimationTime))]
    public class CheckAnimationTime : Condition
    {
        [SerializeField] protected float _percentage;

        public override bool CheckCondition(BaseCreature target)
        {
            var normalTime = target.HFSM.GetCurrentAnimationNormalizedTime();
            if (_percentage < normalTime)
                return true;
            return false;
        }

        public override Condition GetCopy()
        {
            var copy = CreateInstance<CheckAnimationTime>();
            copy._percentage = _percentage;
            return copy;
        }
    }
}