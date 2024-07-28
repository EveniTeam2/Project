using System;
using Unit.GameScene.Manager.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public abstract class Card
    {
        protected string title;
        protected string description;
        protected Sprite image;
        protected ERarity rarity;
        protected int levelStartFrom1;
        protected int maxLevel;
        public string Title { get => title; }
        public string Description { get => description; }
        public Sprite Image { get => image; }
        public ERarity Rarity { get => rarity; }
        public int LevelStartFrom1 { get => levelStartFrom1; }
        public int MaxLevel { get => maxLevel; }
        public event Action<Card, int> OnLevelUP;

        protected Card(CardData cardData)
        {
            title = cardData.Title;
            description = cardData.Description;
            image = cardData.Image;
            rarity = cardData.Rarity;
            maxLevel = cardData.MaxLevel;
            levelStartFrom1 = 1;
        }

        public abstract void Apply(IStage stage);
        public virtual void LevelUp()
        {
            OnLevelUP?.Invoke(this, levelStartFrom1);
        }
    }

    public abstract class CardData : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private Sprite image;
        [SerializeField] private ERarity rarity;
        [SerializeField] private int maxLevel;
        public string Title { get => title; }
        public string Description { get => description; }
        public Sprite Image { get => image; }
        public ERarity Rarity { get => rarity; }
        public int MaxLevel { get => maxLevel; }

        public abstract Card GetCard();
    }
}