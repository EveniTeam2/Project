using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Cards.Abstract
{
    public class CardView : MonoBehaviour, IPointerClickHandler
    {
        private event Action OnClickCard;
        
        [SerializeField] private Image cardIcon;
        [SerializeField] private TextMeshProUGUI cardDescription;

        public void Initialize(Sprite icon, string description, Action action)
        {
            cardIcon.sprite = icon;
            cardDescription.text = description;

            OnClickCard += action;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickCard.Invoke();
        }
    }
}