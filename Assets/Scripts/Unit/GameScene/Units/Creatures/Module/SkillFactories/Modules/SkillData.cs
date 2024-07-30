using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules
{
    public struct SkillData
    {
        public CharacterClassType CharacterType { get; set; }
        public int SkillIndex { get; set; }
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public int SkillLevel { get; set; }
        public int SkillValue { get; set; }
        public float SkillRange1 { get; set; }
        public float SkillRange2 { get; set; }
        public SkillType SkillType { get; set; }
        public bool IsSingleTarget { get; set; }
        public SkillRangeType SkillRangeType { get; set; }
        public SkillExtraEffectType SkillExtraEffectType { get; set; }
        
        public SkillData(CharacterClassType characterType, int skillIndex, string skillName, string skillDescription, int skillLevel, int skillValue, float skillRange1,  float skillRange2,  SkillType skillType,  bool isSingleTarget, SkillRangeType skillRangeType, SkillExtraEffectType skillExtraEffectType) 
        {
            CharacterType = characterType;
            SkillIndex = skillIndex;
            SkillName = skillName;
            SkillDescription = skillDescription;
            SkillLevel = skillLevel;
            SkillValue = skillValue;
            SkillRange1 = skillRange1;
            SkillRange2 = skillRange2;
            SkillType = skillType;
            IsSingleTarget = isSingleTarget;
            SkillRangeType = skillRangeType;
            SkillExtraEffectType = skillExtraEffectType;
        }
    }
}