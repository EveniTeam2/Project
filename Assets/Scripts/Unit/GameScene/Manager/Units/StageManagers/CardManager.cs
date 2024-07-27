using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class CardManager
    {
        private LinkedList<CardGachaPair> cardGachaPool;
        private int totalWeight;

        public CardManager(CardGachaPair[] cardGachaPool)
        {
            foreach (var cardGachaPair in cardGachaPool)
                this.cardGachaPool.AddLast(cardGachaPair);
        }

        public Card[] GetCards(int count)
        {
            totalWeight = cardGachaPool.Sum(item => item.Weight);
            List<Card> gacha = new List<Card>();

            for (int i = 0; i < count; ++i)
            {
                int rand = Random.Range(0, totalWeight + 1);
                gacha.Add(GetCardGacha(rand));
            }
            return gacha.ToArray();
        }

        public void RemoveCardsFromPool(params Card[] cards)
        {
            foreach (var card in cards)
            {
                RemoveCardFromPool(card);
            }
        }

        private Card GetCardGacha(int randValue)
        {
            int weight = 0;
            foreach (var gacha in cardGachaPool)
            {
                weight += gacha.Weight;
                if (weight > randValue)
                {
                    return gacha.Card;
                }
            }
            return cardGachaPool.Last.Value.Card;
        }

        private void RemoveCardFromPool(Card card)
        {
            LinkedListNode<CardGachaPair> target = cardGachaPool.First;
            while (target != null)
            {
                if (target.Value.Card == card)
                {
                    cardGachaPool.Remove(target);
                    break;
                }
                target = target.Next;
            }
        }
    }

    public class CardGachaPair
    {
        public Card Card { get; private set; }
        public int Weight { get; private set; }
    }

    public enum ERarity
    {
        Normal,
        Rare,
        Epic,
    }

    public abstract class Card
    {
        private string title;
        private string description;
        private Sprite image;
        private ERarity rarity;

        public string Title { get => title; }
        public string Description { get => description; }
        public Sprite Image { get => image; }
        public ERarity Rarity { get => rarity; }

        protected Card(CardData cardData)
        {
            title = cardData.Title;
            description = cardData.Description;
            image = cardData.Image;
            rarity = cardData.Rarity;
        }

        public abstract void Apply(IStage stage);
    }

    public class CardData : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private Sprite image;
        [SerializeField] private ERarity rarity;

        public string Title { get => title; }
        public string Description { get => description; }
        public Sprite Image { get => image; }
        public ERarity Rarity { get => rarity; }
    }
}