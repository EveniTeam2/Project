using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.interfaces;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class LightAttack : CharacterSkill, IKnightSkill
    {
        public KnightSkillType KnightSkillType { get; }
        
        private Character _character;
        
        public LightAttack(CharacterType characterType, KnightSkillType skillType) : base(characterType)
        {
            KnightSkillType = skillType;
        }
        
        public override void ActivateSkill()
        {
            
        }
    }
}