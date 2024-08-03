using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.UI;
using Unit.GameScene.Units.Creatures.Interfaces;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class CardManager
    {
        private Dictionary<string, CardGachaPair> cardGachaPool;
        private Dictionary<string, CardGachaPair> defaultGachaPool;
        private int totalWeight;
        private readonly UICardPanel ui;
        private readonly StageManager stage;

        public CardManager(UICardPanel ui, StageManager stage, CardGachaPair[] defaultCardPool, params CardGachaPair[] cardGachas)
        {
            cardGachaPool = new Dictionary<string, CardGachaPair>(cardGachas.Length);
            foreach (var cardGachaPair in cardGachas)
            {
                cardGachaPool.Add(cardGachaPair.Card.Title, cardGachaPair);
                cardGachaPair.Card.OnLevelUP += CheckLevelToRemove;
            }
            totalWeight = cardGachaPool.Values.Sum(card => card.Weight);
            this.ui = ui;
            this.stage = stage;

            defaultGachaPool = new Dictionary<string, CardGachaPair>(defaultCardPool.Length);
            foreach (var card in defaultCardPool)
                defaultGachaPool.Add(card.Card.Title, card);
        }

        public Card[] GetCards(int count)
        {
            LinkedList<CardGachaPair> gacha = new LinkedList<CardGachaPair>();
            foreach (var cardGachaPair in cardGachaPool.Values)
                gacha.AddLast(cardGachaPair);

            List<Card> hand = new List<Card>();

            for (int i = 0; i < count && gacha.Count > 0; ++i)
            {
                int rand = UnityEngine.Random.Range(0, totalWeight + 1);
                var cardGachaPair = GetCardGacha(gacha, rand);
                gacha.Remove(cardGachaPair);
                hand.Add(cardGachaPair.Card);
            }

            if (hand.Count < count)
            {
                LinkedList<CardGachaPair> gachaPool = new LinkedList<CardGachaPair>();
                foreach (var defaultPool in defaultGachaPool.Values)
                    gachaPool.AddLast(defaultPool);

                int targetCount = count - hand.Count;
                for (int i = 0; i < targetCount && gachaPool.Count > 0; ++i)
                {
                    int rand = UnityEngine.Random.Range(0, totalWeight + 1);
                    var cardGachaPair = GetCardGachaFromDefault(gachaPool, rand);
                    gachaPool.Remove(cardGachaPair);
                    hand.Add(cardGachaPair.Card);
                }
            }

            ui.DrawCardButton(hand.ToArray(), SelectCard);

            return hand.ToArray();
        }

        private void SelectCard(Card card)
        {
            card.Apply(stage);
            ui.CloseUI();
        }

        public void RemoveCardsFromPool(params string[] titles)
        {
            foreach (var card in titles)
            {
                RemoveCardFromPool(card);
            }
            totalWeight = cardGachaPool.Values.Sum(card => card.Weight);
        }

        private void DrawCard()
        {
            var cards = GetCards(3);
        }

        public void AddCardsToPool(params CardGachaPair[] cards)
        {
            foreach (var card in cards)
            {
                AddCardToPool(card);
            }
            totalWeight = cardGachaPool.Values.Sum(card => card.Weight);
        }

        public bool TryChangeCardWeight(string title, int weight)
        {
            if (cardGachaPool.TryGetValue(title, out var pair))
            {
                pair.SetWeight(weight);
                totalWeight = cardGachaPool.Values.Sum(card => card.Weight);
                return true;
            }
            return false;
        }

        private void AddCardToPool(CardGachaPair card)
        {
            cardGachaPool.Add(card.Card.Title, card);
            card.Card.OnLevelUP += CheckLevelToRemove;
        }

        private CardGachaPair GetCardGacha(LinkedList<CardGachaPair> gachaPool, int randValue)
        {
            int weight = 0;
            foreach (var gacha in gachaPool)
            {
                weight += gacha.Weight;
                if (weight > randValue)
                {
                    return gacha;
                }
            }
            return gachaPool.Last.Value;
        }

        private CardGachaPair GetCardGachaFromDefault(LinkedList<CardGachaPair> gachaPool, int randValue)
        {
            int weight = 0;
            foreach (var gacha in gachaPool)
            {
                weight += gacha.Weight;
                if (weight > randValue)
                {
                    return gacha;
                }
            }
            return gachaPool.Last.Value;
        }

        private void RemoveCardFromPool(string title)
        {
            if (cardGachaPool.TryGetValue(title, out var cardpair))
            {
                cardGachaPool[title].Card.OnLevelUP -= CheckLevelToRemove;
                cardGachaPool.Remove(title);
            }
        }

        private void CheckLevelToRemove(Card card, int level)
        {
            if (card.MaxLevel <= level)
                RemoveCardFromPool(card.Title);
        }

        public void HandleOnTriggerCard()
        {
            throw new NotImplementedException();
        }
    }

    public class CardGachaPair
    {
        public Card Card { get; private set; }
        public int Weight { get; private set; }

        public CardGachaPair(Card card, int weight)
        {
            Card = card;
            Weight = weight;
        }

        public void SetWeight(int weight)
        {
            Weight = weight;
        }
    }

    public enum ERarity
    {
        Normal,
        Rare,
        Epic,
    }
}