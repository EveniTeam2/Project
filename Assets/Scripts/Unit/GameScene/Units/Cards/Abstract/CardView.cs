using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Cards.Abstract
{
    public class CardView : MonoBehaviour, IPointerClickHandler
    {
        private event Action<int> OnClickCard;
        
        [SerializeField] private Image cardIcon;
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI cardDescription;
        [SerializeField] private List<GameObject> cardGoldStars;
        [SerializeField] private List<GameObject> cardSilverStars;

        private int _index;

        public void Initialize(Sprite icon, string name, string description, int index, int currentLevel, int maxLevel, Action<int> action)
        {
            cardIcon.sprite = icon;
            cardName.text = name;
            cardDescription.text = description;
            _index = index;

            foreach (var goldStar in cardGoldStars)
            {
                goldStar.SetActive(false);
            }
            
            foreach (var silverStar in cardSilverStars)
            {
                silverStar.SetActive(false);
            }

            OnClickCard = action;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"{_index + 1}번째 카드 클릭!");
            OnClickCard.Invoke(_index);
        }
    }
}