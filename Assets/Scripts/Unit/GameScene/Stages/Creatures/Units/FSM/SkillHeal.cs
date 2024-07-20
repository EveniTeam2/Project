using Unit.GameScene.Stages.Creatures.Module;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class SkillHeal : ISkillAct
    {
        private HealthSystem _healthSystem;
        private int _healAmount;

        public SkillHeal(HealthSystem healthSystem, int healAmount)
        {
            _healthSystem = healthSystem;
            _healAmount = healAmount;
        }


        public void Act()
        {
            _healthSystem.Heal(_healAmount);
        }
    }
}