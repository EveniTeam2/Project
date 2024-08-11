using ScriptableObjects.Scripts.Creature.Data.MonsterData;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;

namespace Unit.GameScene.Units.Creatures.Data.MonsterDatas
{
    public class MonsterData
    {
        public MonsterDataSo MonsterDataSo { get; }
        public MonsterStatSystem MonsterStatSystem { get; }
        public MonsterSkillSystem MonsterSkillSystem { get; }

        public MonsterData(MonsterDataSo monsterDataSo, MonsterStatSystem monsterStatSystem, MonsterSkillSystem monsterSkillSystem)
        {
            MonsterDataSo = monsterDataSo;
            MonsterStatSystem = monsterStatSystem;
            MonsterSkillSystem = monsterSkillSystem;
        }
    }
}