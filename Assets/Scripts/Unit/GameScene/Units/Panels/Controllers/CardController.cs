using System;
using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Modules;
using Unit.GameScene.Units.Panels.Interfaces;
using Unit.GameScene.Units.Panels.Modules.CardModules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Enums;
using UnityEngine;
using Random = System.Random;

namespace Unit.GameScene.Units.Panels.Controllers
{
    public class CardController : MonoBehaviour
    {
        private int _cardTrigger;
        private RectTransform _cardPanel;
        private RectTransform _cardSpawnPanel;
        private HashSet<Card> _cardInfo;

        [Header("카드 생성 개수"), SerializeField]
        private int poolSize;

        [Header("카드 풀링 관련 설정"), SerializeField]
        private CardView cardViewPrefab;
        private ICardPool _cardPool;
        private List<Card> _targetCards;
        private List<CardView> _cardViews;
        private Dictionary<BlockType, CharacterSkill> _blockInfo;

        private bool _cardTriggerIsRunning;
        private bool _isCardClicked;

        private CardClickHandler _cardClickHandler;

        public void Initialize(RectTransform cardPanel, RectTransform cardSpawnPanel, HashSet<Card> cardInfo, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            _cardPanel = cardPanel;
            _cardInfo = cardInfo;
            _cardSpawnPanel = cardSpawnPanel;
            _cardTrigger = 0;
            _cardTriggerIsRunning = false;
            _isCardClicked = false;
            _blockInfo = blockInfo;
            _targetCards = new List<Card>();
            _cardViews = new List<CardView>();

            _cardPool = new CardPool(cardViewPrefab, cardSpawnPanel, poolSize, true);
            _cardClickHandler = new CardClickHandler(_cardInfo, _blockInfo, _cardPool, _targetCards, _cardViews, SetActiveCardPanel, UpdateCardTriggerRunner);
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
            GetCard();
            SetActiveCardPanel(true);
            Debug.Log("카드 드로우!!!");
        }

        private void GetCard()
        {
            _cardTrigger--;
            Debug.Log($"현재 트리거 카운트 : {_cardTrigger}");
            
            var random = new Random();
            var cardList = _cardInfo.ToList();
            var count = Math.Min(poolSize, cardList.Count);

            for (var i = 0; i < count; i++)
            {
                var cardView = _cardPool.Get();
                int randomIndex = random.Next(cardList.Count);
                var targetCard = cardList[randomIndex];
                cardList.RemoveAt(randomIndex);
                
                _cardViews.Add(cardView);
                _targetCards.Add(targetCard);
                
                cardView.Initialize(targetCard.CardIcon, targetCard.CardName, targetCard.CardDescription, i, index => _cardClickHandler.HandleOnClickCard(index, ref _isCardClicked));
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

        public void RegisterHandleOnRegisterCharacterSkill(Func<CharacterSkill, SkillRegisterType> func)
        {
            _cardClickHandler.RegisterHandleOnRegisterCharacterSkill(func);
        }
    }
}
