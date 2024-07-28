using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class CardManager
    {
        private Dictionary<string, CardGachaPair> cardGachaPool;
        private int totalWeight;

        public CardManager(params CardGachaPair[] cardGachas)
        {
            cardGachaPool = new Dictionary<string, CardGachaPair>();
            foreach (var cardGachaPair in cardGachas)
            {
                this.cardGachaPool.Add(cardGachaPair.Card.Title, cardGachaPair);
                cardGachaPair.Card.OnLevelUP += CheckLevelToRemove;
            }
            totalWeight = cardGachaPool.Values.Sum(card => card.Weight);
        }

        public Card[] GetCards(int count)
        {
            List<Card> gacha = new List<Card>();

            for (int i = 0; i < count; ++i)
            {
                int rand = UnityEngine.Random.Range(0, totalWeight + 1);
                gacha.Add(GetCardGacha(rand));
            }
            return gacha.ToArray();
        }

        public void RemoveCardsFromPool(params string[] titles)
        {
            foreach (var card in titles)
            {
                RemoveCardFromPool(card);
            }
            totalWeight = cardGachaPool.Values.Sum(card => card.Weight);
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

        private Card GetCardGacha(int randValue)
        {
            int weight = 0;
            var pairs = cardGachaPool.Values.ToArray();
            foreach (var gacha in pairs)
            {
                weight += gacha.Weight;
                if (weight > randValue)
                {
                    return gacha.Card;
                }
            }
            return pairs[pairs.Length-1].Card;
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