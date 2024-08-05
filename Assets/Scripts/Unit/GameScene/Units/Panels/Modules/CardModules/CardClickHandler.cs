using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Units;
using Unit.GameScene.Units.Panels.Interfaces;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Enums;

namespace Unit.GameScene.Units.Panels.Modules.CardModules
{
    public class CardClickHandler
    {
        private Func<CharacterSkill, SkillRegisterType> _onRegisterCharacterSkill;
        
        private readonly Dictionary<BlockType, CharacterSkill> _blockInfo;
        private readonly ICardPool _cardPool;
        private readonly List<Card> _targetCards;
        private readonly List<CardView> _cardViews;
        private readonly Action<bool> _setActiveCardPanel;
        private readonly Action<bool> _updateCardTriggerRunner;
        
        private HashSet<Card> _cardInfo;

        public CardClickHandler(
            HashSet<Card> cardInfo,
            Dictionary<BlockType, CharacterSkill> blockInfo,
            ICardPool cardPool,
            List<Card> targetCards,
            List<CardView> cardViews,
            Action<bool> setActiveCardPanel,
            Action<bool> updateCardTriggerRunner)
        {
            _cardInfo = cardInfo;
            _blockInfo = blockInfo;
            _cardPool = cardPool;
            _targetCards = targetCards;
            _cardViews = cardViews;
            _setActiveCardPanel = setActiveCardPanel;
            _updateCardTriggerRunner = updateCardTriggerRunner;
        }

        public void HandleOnClickCard(int index, ref bool isCardClicked)
        {
            if (isCardClicked) return;
            isCardClicked = true;

            var targetCard = _targetCards[index];

            if (targetCard is ActiveSkillCard activeSkillCard)
            {
                var result = _onRegisterCharacterSkill(activeSkillCard.CharacterSkill);

                switch (result)
                {
                    case SkillRegisterType.Success:
                        break;
                    case SkillRegisterType.Full:
                        _cardInfo = FilterCardInfo(_cardInfo, _blockInfo);
                        break;
                    case SkillRegisterType.IsExisted:
                        if (!targetCard.ActivateCardEffect())
                        {
                            _cardInfo.Remove(_targetCards[index]);
                        }
                        break;
                }
            }
            else
            {
                if (targetCard.ActivateCardEffect())
                {
                    _cardInfo.Remove(_targetCards[index]);
                }
            }

            ReleaseCardViews();
            _targetCards.Clear();
            _setActiveCardPanel(false);
            _updateCardTriggerRunner(false);
            
            isCardClicked = false;
        }

        private HashSet<Card> FilterCardInfo(HashSet<Card> cardInfo, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            var tempCardInfo = new HashSet<Card>();
            
            foreach (var card in cardInfo)
            {
                if (card.CardTargetType != CardTargetType.Skill)
                {
                    tempCardInfo.Add(card);
                }
                else if (card is ActiveSkillCard activeSkillCard)
                {
                    foreach (var block in blockInfo)
                    {
                        if (block.Value == activeSkillCard.CharacterSkill)
                        {
                            tempCardInfo.Add(card);
                        }
                    }
                }
            }
            
            return tempCardInfo;
        }

        private void ReleaseCardViews()
        {
            foreach (var cardView in _cardViews)
            {
                _cardPool.Release(cardView);
            }
        }
        
        public void RegisterHandleOnRegisterCharacterSkill(Func<CharacterSkill, SkillRegisterType> func)
        {
            _onRegisterCharacterSkill = func;
        }
    }
}
