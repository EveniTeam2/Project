namespace Unit.GameScene.Units.Creatures.Interfaces.SkillController
{
    public interface IGetSkillInfo
    {
        int GetSkillIndex(string skillName);
        int GetSkillValue(string skillName);
        float GetSkillRange(string skillName);
    }
}