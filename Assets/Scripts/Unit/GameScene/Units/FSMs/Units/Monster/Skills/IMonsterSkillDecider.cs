namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public interface IMonsterSkillDecider
    {
        bool CanExcute();
        void ResetDecider();
    }
}