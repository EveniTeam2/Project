namespace Unit.GameScene.Units.Creatures.Module.Systems.Abstract
{
    public interface IBattleStat
    {
        float GetCoolTime();
        int GetAttack();
        
        int GetSkillIndex(string skillName);
        float GetSkillRange(string skillName);
        int GetSkillValue(string skillName);
    }
}