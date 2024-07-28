using ScriptableObjects.Scripts.Creature.Data;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterData
    {
        public CharacterDataSo CharacterDataSo { get; }
        public SkillManager SkillManager { get; }
        public StatManager StatManager { get; }

        public CharacterData(CharacterDataSo characterDataSo, StatManager statManager, SkillManager skillManager)
        {
            CharacterDataSo = characterDataSo;
            StatManager = statManager;
            SkillManager = skillManager;
        }

        public void RegisterCharacterServiceProvider(ICharacterServiceProvider creatureServiceProvider)
        {
            SkillManager.RegisterCharacterServiceProvider(creatureServiceProvider);
        }
    }
}