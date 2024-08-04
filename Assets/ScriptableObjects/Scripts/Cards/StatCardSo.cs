using ScriptableObjects.Scripts.Creature.Data;
using System.Collections.Generic;
using Unit.GameScene.Units.Cards.Abstract;
using UnityEngine;

namespace ScriptableObjects.Scripts.Cards
{
    [CreateAssetMenu(fileName = nameof(StatCardSo), menuName = nameof(Card) + "/" + nameof(StatCardSo))]
    public class StatCardSo : ScriptableObject
    {
        [Header("스탯 카드 이미지")]
        public List<Sprite> statSprite;
    }
}