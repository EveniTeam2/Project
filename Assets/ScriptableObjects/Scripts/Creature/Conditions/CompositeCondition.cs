using Unit.GameScene.Stages.Creautres;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Conditions
{
    [CreateAssetMenu(fileName = nameof(CompositeCondition), menuName = "State/Condition/" + nameof(CompositeCondition))]
    public class CompositeCondition : Condition
    {
        [SerializeField] private Condition[] conditions;

        public override bool CheckCondition(BaseCreature target)
        {
            for (var i = 0; i < conditions.Length; ++i)
                if (!conditions[i].CheckCondition(target))
                    return false;
            return true;
        }

        public override Condition GetCopy()
        {
            var copy = CreateInstance<CompositeCondition>();

            for (var i = 0; i < conditions.Length; i++)
            {
                copy.conditions[i] = conditions[i];
            }
            
            return copy;
        }
    }
}