using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unit.GameScene.Units.Cards.Enums;
using Unit.GameScene.Units.Cards.Units;
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
        [SerializeField] private List<StarView> stars;
        
        private int _index;

        public void Initialize(Sprite cIcon, string cName, string cDescription, CardLevelType cType, int currentLevel, int maxLevel, int cIndex, Action<int> cAction)
        {
            cardIcon.sprite = cIcon;
            cardName.text = cName;
            cardDescription.text = cDescription;
            _index = cIndex;

            OnClickCard = cAction;

            SetActiveStars(cType, currentLevel, maxLevel);
        }

        // TODO : 이후 CardView를 추상화하여 ActiveCardView, PassiveCardView로 분리해야 할 듯
        private void SetActiveStars(CardLevelType type, int currentLevel, int maxLevel)
        {
            switch (type)
            {
                case CardLevelType.Passive:
                    for (var i = 1; i <= stars.Count; i++)
                    {
                        if (i <= maxLevel)
                        {
                            stars[i - 1].Initialize(StarType.GoldStar);
                            stars[i - 1].gameObject.SetActive(true);
                        }
                    }
                    break;
                case CardLevelType.Active:
                    for (var i = 1; i <= stars.Count; i++)
                    {
                        if (i <= currentLevel)
                        {
                            stars[i - 1].Initialize(StarType.GoldStar);
                            stars[i - 1].gameObject.SetActive(true);
                        }
                        else if(i <= maxLevel)
                        {
                            stars[i - 1].Initialize(StarType.SilverStar);
                            stars[i - 1].gameObject.SetActive(true);
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
            }
        }

        private void OnDisable()
        {
            foreach (StarView star in stars.Where(star => star.gameObject.activeInHierarchy))
            {
                star.gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"{_index + 1}번째 카드 클릭!");
            OnClickCard.Invoke(_index);
        }
    }
}