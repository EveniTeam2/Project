using System.Collections.Generic;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Data;
using Unit.GameScene.Units.Cards.Units;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;

namespace Unit.GameScene.Units.CardFactories.Units
{
    public class CardFactory
    {
        private List<StatCardData> _csvData;
        private Dictionary<string, CharacterSkill> _skillData;
        
        public CardFactory(List<StatCardData> csvData, Dictionary<string, CharacterSkill> skillData)
        {
            _csvData = csvData;
            _skillData = skillData;
        }

        public Dictionary<string, Card> CreateCard()
        {
            var cards = new Dictionary<string, Card>();

            return cards;
        }
    }
}