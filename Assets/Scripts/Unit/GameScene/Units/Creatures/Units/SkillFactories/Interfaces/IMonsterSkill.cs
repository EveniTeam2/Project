using Unit.GameScene.Stages.Creatures.Units.Monsters.Enums;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces
{
    public interface IMonsterSkill
    {
        public MonsterType MonsterType { get; set; }
    }
}