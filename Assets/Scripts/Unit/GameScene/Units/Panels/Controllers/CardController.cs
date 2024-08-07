using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Modules;
using Unit.GameScene.Units.Cards.Units;
using Unit.GameScene.Units.Panels.Interfaces;
using Unit.GameScene.Units.Panels.Modules.CardModules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Enums;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Units.Panels.Controllers
{
    public class CardController : MonoBehaviour
    {
        [Header("카드 생성 개수"), SerializeField]
        private int cardPoolSize;

        [Header("별 생성 개수"), SerializeField]
        private int cardStarPoolSize;

        [Header("카드 풀링 관련 설정"), SerializeField]
        private CardView cardViewPrefab;

        [FormerlySerializedAs("cardStarViewPrefab"), Header("별 풀링 관련 설정"), SerializeField]
        private StarView starViewPrefab;

        private int _cardTrigger;
        private RectTransform _cardPanel;
        private RectTransform _cardSpawnPanel;
        private HashSet<Card> _cardInfo;

        private ICardPool _cardPool;
        private List<Card> _targetCards;
        private List<CardView> _cardViews;
        private List<StarView> _cardStarViews;
        private Dictionary<BlockType, CharacterSkill> _blockInfo;

        private bool _cardTriggerIsRunning;
        private bool _isCardClicked;

        private CardClickHandler _cardClickHandler;

        public void Initialize(
            RectTransform cardPanel,
            RectTransform cardSpawnPanel,
            HashSet<Card> cardInfo,
            Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            _cardPanel = cardPanel;
            _cardInfo = cardInfo;
            _cardSpawnPanel = cardSpawnPanel;
            _blockInfo = blockInfo;
            _cardTrigger = 0;
            _cardTriggerIsRunning = false;
            _isCardClicked = false;

            _targetCards = new List<Card>();
            _cardViews = new List<CardView>();
            _cardStarViews = new List<StarView>();

            _cardPool = new CardPool(cardViewPrefab, cardSpawnPanel, cardPoolSize, true);
            
            // CardClickHandler 초기화 시 _cardInfo의 참조를 전달
            _cardClickHandler = new CardClickHandler(
                _cardInfo, // 참조 전달
                _blockInfo,
                _cardPool,
                _targetCards,
                _cardViews,
                _cardStarViews,
                SetActiveCardPanel,
                UpdateCardTriggerRunner);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleOnTriggerCard();
            }

            if (_cardTrigger > 0 && !_cardTriggerIsRunning)
            {
                DrawCard();
            }
        }

        private void DrawCard()
        {
            UpdateCardTriggerRunner(true);
            _cardTrigger--;
            Debug.Log($"현재 트리거 카운트 : {_cardTrigger}");

            GetCard();
            SetActiveCardPanel(true);
            Debug.Log("카드 드로우!!!");
        }

        private void GetCard()
        {
            var random = new System.Random();
            var cardList = _cardInfo.ToList();
            var count = Math.Min(cardPoolSize, cardList.Count);

            _targetCards.Clear();
            
            for (var i = 0; i < count; i++)
            {
                var cardView = _cardPool.Get();
                int randomIndex = random.Next(cardList.Count);
                var targetCard = cardList[randomIndex];

                int targetCardCurrentLevel;
                int targetCardMaxLevel;

                switch (targetCard.CardLevelType)
                {
                    case CardLevelType.Active:
                        var obj = targetCard as ActiveCard;
                        targetCardCurrentLevel = obj.CardCurrentLevel;
                        targetCardMaxLevel = obj.CardMaxLevel;
                        break;
                    case CardLevelType.Passive:
                        targetCardCurrentLevel = 5;
                        targetCardMaxLevel = 5;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                cardList.RemoveAt(randomIndex);

                _cardViews.Add(cardView);
                _targetCards.Add(targetCard);

                cardView.Initialize(
                    targetCard.CardIcon,
                    targetCard.CardName,
                    targetCard.CardDescription,
                    targetCard.CardLevelType,
                    targetCardCurrentLevel,
                    targetCardMaxLevel,
                    i,
                    index => _cardClickHandler.HandleOnClickCard(index, ref _isCardClicked));
            }
        }

        public void HandleOnTriggerCard()
        {
            _cardTrigger++;
        }

        private void SetActiveCardPanel(bool value)
        {
            _cardPanel.gameObject.SetActive(value);
        }

        private void UpdateCardTriggerRunner(bool value)
        {
            Time.timeScale = value ? 0 : 1;
            _cardTriggerIsRunning = value;
        }

        public void RegisterHandleOnRegisterCharacterSkill(Action<BlockType> onUpdateCharacterSkillOnBlock)
        {
            _cardClickHandler.RegisterHandleOnRegisterCharacterSkill(onUpdateCharacterSkillOnBlock);
        }
    }
}