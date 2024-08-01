using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.SkillFactories.Modules
{
    public struct SkillData
    {
        public CharacterClassType CharacterType { get; }
        public int SkillIndex { get; }
        public string SkillName { get; }
        public string SkillDescription { get; }
        public int SkillLevel { get; }
        public int SkillValue { get; }
        public float SkillRange1 { get; }
        public float SkillRange2 { get; }
        public SkillType SkillType { get; }
        public bool IsSingleTarget { get; }
        public SkillRangeType SkillRangeType { get; }
        public SkillExtraEffectType SkillExtraEffectType { get; }
        
        public SkillData(CharacterClassType characterType, int skillIndex, string skillName, string skillDescription, int skillLevel, int skillValue, float skillRange1,  float skillRange2,  SkillType skillType, int isSingleTarget, SkillRangeType skillRangeType, SkillExtraEffectType skillExtraEffectType) 
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
            IsSingleTarget = isSingleTarget == 0;
            SkillRangeType = skillRangeType;
            SkillExtraEffectType = skillExtraEffectType;
        }
    }
}