using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Units;
using Unit.GameScene.Units.Panels.Interfaces;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Enums;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Units.Panels.Modules.CardModules
{
    public class CardClickHandler
    {
        private readonly Dictionary<BlockType, CharacterSkill> _blockInfo;
        private readonly ICardPool _cardPool;
        private readonly List<Card> _targetCards;
        private readonly List<CardView> _cardViews;
        private readonly List<StarView> _cardStarViews;
        private readonly Action<bool> _setActiveCardPanel;
        private readonly Action<bool> _updateCardTriggerRunner;
        private readonly HashSet<Card> _cardInfo;

        private Action<BlockType> _onUpdateCharacterSkillOnBlock;

        public CardClickHandler(
            HashSet<Card> cardInfo,
            Dictionary<BlockType, CharacterSkill> blockInfo,
            ICardPool cardPool,
            List<Card> targetCards,
            List<CardView> cardViews,
            List<StarView> cardStarViews,
            Action<bool> setActiveCardPanel,
            Action<bool> updateCardTriggerRunner)
        {
            _cardInfo = cardInfo;
            _blockInfo = blockInfo;
            _cardPool = cardPool;
            _targetCards = targetCards;
            _cardViews = cardViews;
            _cardStarViews = cardStarViews;
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
                var result = RegisterCharacterSkill(activeSkillCard.CharacterSkill);

                switch (result)
                {
                    case SkillRegisterType.Success:
                        targetCard.ActivateCardEffect();
;                        break;
                    case SkillRegisterType.Full:
                        FilterCardInfo(); // 메서드 내에서 _cardInfo와 _blockInfo를 직접 사용하도록 변경
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

        /// <summary>
        /// 블록에 스킬을 등록하고, 성공 시 OnUpdateCharacterSkillOnBlock 이벤트를 호출합니다.
        /// </summary>
        private SkillRegisterType RegisterCharacterSkill(CharacterSkill characterSkill)
        {
            var blockTypeLength = Enum.GetValues(typeof(BlockType)).Length - 1;
            
            // _blockInfo에서 characterSkill이 이미 있는지 체크
            for (var i = 0; i < blockTypeLength; i++)
            {
                if (_blockInfo[(BlockType)i] == characterSkill)
                {
                    return SkillRegisterType.IsExisted;
                }
            }

            // _blockInfo에서 null 슬롯을 찾아서 스킬을 등록
            for (var i = 0; i < blockTypeLength; i++)
            {
                var blockType = (BlockType)i;
                if (_blockInfo[blockType] != null) continue;
                
                _blockInfo[blockType] = characterSkill;
                characterSkill.IsRegisterOnBlock = true;
                
                // 성공적으로 스킬을 등록한 경우, 이벤트 호출
                _onUpdateCharacterSkillOnBlock?.Invoke(blockType);
                
                return i == blockTypeLength - 1 ? SkillRegisterType.Full : SkillRegisterType.Success;
            }

            // 등록 가능한 슬롯이 없는 경우
            return SkillRegisterType.Full;
        }

        /// <summary>
        /// 카드 정보를 필터링하여 업데이트합니다.
        /// </summary>
        private void FilterCardInfo()
        {
            var tempCardInfo = new HashSet<Card>();
            
            foreach (var card in _cardInfo)
            {
                if (card.CardTargetType != CardTargetType.Skill)
                {
                    tempCardInfo.Add(card);
                }
                else if (card is ActiveSkillCard activeSkillCard)
                {
                    foreach (var block in _blockInfo)
                    {
                        if (block.Value == activeSkillCard.CharacterSkill)
                        {
                            tempCardInfo.Add(card);
                        }
                    }
                }
            }

            _cardInfo.Clear();
            foreach (var card in tempCardInfo)
            {
                _cardInfo.Add(card);
            }
        }

        /// <summary>
        /// 사용한 카드 뷰를 풀에 반환합니다.
        /// </summary>
        private void ReleaseCardViews()
        {
            foreach (var cardView in _cardViews)
            {
                _cardPool.Release(cardView);
            }
        }
        
        /// <summary>
        /// 블록에 스킬이 등록될 때 호출되는 이벤트 핸들러를 등록합니다.
        /// </summary>
        public void RegisterHandleOnRegisterCharacterSkill(Action<BlockType> onUpdateCharacterSkillOnBlock)
        {
            _onUpdateCharacterSkillOnBlock = onUpdateCharacterSkillOnBlock;
        }
    }
}