using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Modules;
using Unit.GameScene.Units.Panels.Interfaces;
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
        
        private readonly List<Card> targetCards = new ();
        private readonly List<CardView> _cardViews = new ();
        
        private bool cardTriggerIsRunning;
        private bool isCardClicked;
        
        public void Initialize(RectTransform carPanel, RectTransform cardSpawnPanel, HashSet<Card> cardInfo)
        {
            _cardPanel = carPanel;
            _cardInfo = cardInfo;
            _cardSpawnPanel = cardSpawnPanel;
            _cardTrigger = 0;
            cardTriggerIsRunning = false;
            isCardClicked = false;

            _cardPool = new CardPool(cardViewPrefab, cardSpawnPanel, poolSize, true);
        }

        private void Update()
        {
            //TODO : 테스트 목적, 이후에 지워야 함
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleOnTriggerCard();
            }
            
            if (_cardTrigger > 0 && cardTriggerIsRunning == false)
            {
                DrawCard();
            }
        }

        private void DrawCard()
        {
            UpdateCardTriggerRunner(true);
            
            _cardTrigger--;
            
            GetCard();
            SetActiveCardPanel(true);
            
            Debug.Log("카드 드로우!!!");
        }
        
        private void GetCard()
        {
            var random = new Random();
            var cardList = _cardInfo.ToList();
            var count = poolSize <= cardList.Count ? poolSize : cardList.Count;
            
            for (var i = 0; i < count; i++)
            {
                var cardView = _cardPool.Get();
                int randomIndex = random.Next(cardList.Count);
                var targetCard = cardList[randomIndex];
                
                cardView.Initialize(targetCard.CardIcon, targetCard.CardName, targetCard.CardDescription, i, HandleOnClickCard);
                _cardViews.Add(cardView);
                targetCards.Add(targetCard);
            }
        }

        private void HandleOnClickCard(int index)
        {
            if (isCardClicked) return;
            isCardClicked = true;

            var targetCard = targetCards[index];
            
            if (targetCard.ActivateCardEffect())
            {
                _cardInfo.Remove(targetCards[index]);
            }

            foreach (var cardView in _cardViews)
            {
                _cardPool.Release(cardView);
            }
            
            isCardClicked = false;
            targetCards.Clear();

            SetActiveCardPanel(false);
            UpdateCardTriggerRunner(false);
        }
        
        public void HandleOnTriggerCard()
        {
            _cardTrigger++;
        }

        private void SetActiveCardPanel(bool value)
        {
            switch (value)
            {
                case true:
                    if (!_cardPanel.gameObject.activeInHierarchy) _cardPanel.gameObject.SetActive(true);
                    break;
                case false:
                    if (_cardPanel.gameObject.activeInHierarchy) _cardPanel.gameObject.SetActive(false);
                    break;
            }
        }

        private void UpdateCardTriggerRunner(bool value)
        {
            switch (value)
            {
                case true:
                    Time.timeScale = 0;
                    cardTriggerIsRunning = true;
                    break;
                case false:
                    Time.timeScale = 1;
                    cardTriggerIsRunning = false;
                    break;
            }
        }
    }
}