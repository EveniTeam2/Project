using ScriptableObjects.Scripts.Creature.Data;
using Unit.GameScene.Units.Creatures.Interfaces.Battles;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;
using ICharacterSkillController = Unit.GameScene.Units.Creatures.Interfaces.Battles.ICharacterSkillController;

namespace Unit.GameScene.Units.Creatures.Data.CharacterDatas
{
    public class CharacterData
    {
        public CharacterDataSo CharacterDataSo { get; }
        public CharacterStatSystem CharacterStatSystem { get; }
        public CharacterSkillSystem CharacterSkillSystem { get; }

        public CharacterData(CharacterDataSo characterDataSo, CharacterStatSystem characterStatSystem, CharacterSkillSystem characterSkillSystem)
        {
            CharacterDataSo = characterDataSo;
            CharacterStatSystem = characterStatSystem;
            CharacterSkillSystem = characterSkillSystem;
        }

        public void RegisterCharacterServiceProvider(ICharacterSkillController characterSkillController)
        {
            CharacterSkillSystem.RegisterCharacterServiceProvider(characterSkillController);
        }
    }
}