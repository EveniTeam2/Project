namespace Unit.GameScene.Units.Creatures.Interfaces.SkillControllers
{
    public interface ICharacterBattleController
    {
        void HealMySelf(int value);
        void AttackEnemy(int value, float range);
        void Summon();
    }
}