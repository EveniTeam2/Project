using ScriptableObjects.Scripts.Creature.Data;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Systems.CharacterSystems;

namespace Unit.GameScene.Units.Creatures.Data.CharacterDatas
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

        public void RegisterCharacterServiceProvider(ICharacterSkillController characterSkillController)
        {
            SkillSystem.RegisterCharacterServiceProvider(characterSkillController);
        }
    }
}