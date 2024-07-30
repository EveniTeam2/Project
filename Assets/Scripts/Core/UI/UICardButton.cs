using System;
using Unit.GameScene.Manager.Units.StageManagers;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UICardButton : UIBase<UICardButton>
    {
        [SerializeField] protected Button btn;
        [SerializeField] protected TextPair title;
        [SerializeField] private TextPair description;
        [SerializeField] protected Image icon;
        public event Action<Card> OnCardClick;
        private Card card;
        public override UICardButton InitUI()
        {
            return this;
        }

        public override UICardButton DrawUI()
        {
            CallActOnDraw();
            gameObject.SetActive(true);
            return this;
        }

        public override UICardButton CloseUI()
        {
            CallActOnClose();
            gameObject.SetActive(false);
            return this;
        }

        public override void SetFontScale(float fontScale)
        {
            description.text.fontSize = description.fontSize * fontScale;
            title.text.fontSize = title.fontSize * fontScale;
        }

        public override UICardButton UpdateUI()
        {
            return this;
        }

        internal UICardButton DrawCard(Card card)
        {
            OnCardClick = null;
            btn.onClick.RemoveAllListeners();

            this.card = card;
            title.text.text = card.Title;
            description.text.text = card.Description;
            icon.sprite = card.Image;

            btn.onClick.AddListener(CardClicked);
            DrawUI();
            return this;
        }

        private void CardClicked()
        {
            OnCardClick?.Invoke(card);
        }
    }
}