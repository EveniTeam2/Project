using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
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