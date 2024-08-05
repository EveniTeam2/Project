using Core.Utils;
using System;
using Unit.GameScene.Units.Cards.Abstract;
using Unit.GameScene.Units.Panels.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unit.GameScene.Units.Cards.Modules
{
    public class CardPool : CustomPool<CardView>, ICardPool
    {
        public CardPool(CardView prefab, Transform root, int size, bool isFlexible)
            : base(prefab, root, OnCreate, OnGet, OnRelease, OnDestroy, size, isFlexible)
        {
        }
        
        private static void OnCreate(CardView blockView, CustomPool<CardView> blockPool)
        {
            blockView.gameObject.SetActive(false);
        }

        private static void OnGet(CardView blockView)
        {
            blockView.gameObject.SetActive(true);
        }

        private static void OnRelease(CardView blockView)
        {
            blockView.gameObject.SetActive(false);
        }

        private static void OnDestroy(CardView blockView)
        {
            Object.Destroy(blockView.gameObject);
        }
    }
}