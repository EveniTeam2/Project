namespace Unit.GameScene.Units.Creatures.Interfaces.SkillController
{
    public interface ISetBattleInfo
    {
        void HealMySelf(int value);
        void AttackEnemy(int value, float range);
    }
}