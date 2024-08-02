using Core.Utils;
using System;
using TMPro;
using Unit.GameScene.Manager.Units.StageManagers;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UICardPanel : UIBase<UICardPanel>
    {
        [SerializeField] private TextPair[] text;
        [SerializeField] private RectTransform buttonRoot;
        [SerializeField] private UICardButton prefab;
        
        private CustomPool<UICardButton> buttonPool;

        private void DestroyButton(UICardButton button)
        {
            Destroy(button.gameObject);
        }

        private void ReleaseButton(UICardButton button)
        {
            button.gameObject.SetActive(false);
        }

        private void GetButton(UICardButton button)
        {
            
        }

        private void CreateButton(UICardButton button, CustomPool<UICardButton> pool)
        {
            button.InitUI();
            button.gameObject.SetActive(false);
        }

        public override UICardPanel CloseUI()
        {
            CallActOnClose();
            buttonPool.ReleaseAll();
            gameObject.SetActive(false);
            return this;
        }

        public UICardPanel DrawCardButton(Card[] cards, Action<Card> onClick)
        {
            foreach (var card in cards)
            {
                buttonPool.Get().DrawCard(card).OnCardClick += onClick;
            }
            DrawUI();
            return this;
        }

        public override UICardPanel DrawUI()
        {
            CallActOnDraw();
            gameObject.SetActive(true);
            return this;
        }

        public override UICardPanel InitUI()
        {
            buttonPool = new CustomPool<UICardButton>(prefab, buttonRoot, CreateButton, GetButton, ReleaseButton, DestroyButton, 5, false);
            return this;
        }

        public override void SetFontScale(float fontScale)
        {
            foreach (var pair in text)
            {
                pair.text.fontSize = pair.fontSize * fontScale;
            }
        }

        public override void SetScale(float scale)
        {
            
        }

        public override UICardPanel UpdateUI()
        {
            return this;
        }
    }
}