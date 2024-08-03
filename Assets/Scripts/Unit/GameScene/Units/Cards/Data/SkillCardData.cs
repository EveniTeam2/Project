using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Cards.Data
{
    public class SkillCardData : CardData
    {
        public readonly CharacterSkill CharacterSkill;
        
        public SkillCardData(string cardName, string cardDescription, Sprite cardImage, int cardLevel, CharacterSkill characterSkill) : base(cardName, cardDescription, cardImage, cardLevel)
        {
            CharacterSkill = characterSkill;
        }
    }
}