using ScriptableObjects.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Data;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Interfaces;
using Unit.GameScene.Units.Cards.Units;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;

namespace Unit.GameScene.Units.CardFactories.Units
{
    public class CardFactory
    {
        private readonly List<StatCardData> _statCardData;
        private readonly Dictionary<string, CharacterSkill> _skillData;
        private readonly StatCardSo _statCardSos;
        private readonly IUpdateCreatureStat _character;
        
        public CardFactory(List<StatCardData> statCardData, StatCardSo statCardSos, Dictionary<string, CharacterSkill> skillData, IUpdateCreatureStat character)
        {
            _statCardData = statCardData;
            _statCardSos = statCardSos;
            _skillData = skillData;
            _character = character;
        }

        public HashSet<Card> CreateCard()
        {
            var cards = new HashSet<Card>();
            var statCardSprites = _statCardSos.statSprite;

            for (var i = 0; i < statCardSprites.Count; i++)
            {
                var csvData = _statCardData.Where(data => data.CardIndex == i).ToList();

                Card product = csvData[0].CardLevelType switch
                {
                    CardLevelType.Passive => new PassiveStatCard(statCardSprites[0], csvData[0], _character),
                    CardLevelType.Active => new ActiveStatCard(statCardSprites[0], csvData, _character),
                    _ => throw new ArgumentOutOfRangeException()
                };

                cards.Add(product);
            }
            
            foreach (var data in _skillData)
            {
                var skillCard = new ActiveSkillCard(data.Value);
                cards.Add(skillCard);
            }

            return cards;
        }
    }
}