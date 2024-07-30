using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules
{
    public struct SkillData
    {
        public string SkillId { get; set; }
        public CharacterClassType CharacterType { get; set; }
        public int SkillIndex { get; set; }
        public SkillEffectType SkillEffectType { get; set; }
        public string SkillTypeEnum { get; set; }
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public float SkillRange { get; set; }
        public int SkillEffectValue { get; set; }
        public int SkillLevel { get; set; }
        
        public SkillData(string skillId, CharacterClassType characterType, int skillIndex, SkillEffectType skillEffectType, string skillTypeEnum, string skillName, string skillDescription, float skillRange, int skillEffectValue, int skillLevel)
        {
            SkillId = skillId;
            CharacterType = characterType;
            SkillIndex = skillIndex;
            SkillEffectType = skillEffectType;
            SkillTypeEnum = skillTypeEnum;
            SkillName = skillName;
            SkillDescription = skillDescription;
            SkillRange = skillRange;
            SkillEffectValue = skillEffectValue;
            SkillLevel = skillLevel;
        }
    }
}