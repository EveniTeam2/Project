using ScriptableObjects.Scripts.Creature.Data;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    public class CharacterData
    {
        public CharacterDataSo CharacterDataSo { get; }
        public CharacterSkillSystem SkillSystem { get; }
        public CharacterStatSystem StatSystem { get; }

        public CharacterData(CharacterDataSo characterDataSo, CharacterStatSystem statSystem, CharacterSkillSystem skillSystem)
        {
            CharacterDataSo = characterDataSo;
            StatSystem = statSystem;
            SkillSystem = skillSystem;
        }

        public void RegisterCharacterServiceProvider(ISkillController skillController)
        {
            SkillSystem.RegisterCharacterServiceProvider(skillController);
        }
    }
}