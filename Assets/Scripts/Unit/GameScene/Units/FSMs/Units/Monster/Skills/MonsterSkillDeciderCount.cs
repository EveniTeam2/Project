namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public class MonsterSkillDeciderCount : IMonsterSkillDecider
    {
        private readonly IMonsterSkillAct targetSkill;
        private readonly int count;
        private int current;

        public MonsterSkillDeciderCount(IMonsterSkillAct targetSkill, int count)
        {
            this.targetSkill = targetSkill;
            this.count = count;
            targetSkill.OnExcute += Count;
        }
        private void Count()
        {
            ++current;
        }

        bool IMonsterSkillDecider.CanExcute()
        {
            if (current < count)
                return true;
            return false;
        }

        public void ResetDecider()
        {
            current = 0;
        }
    }
}