using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.GameScene.Units.Cards.Units
{
    public enum StarType
    {
        GoldStar,
        SilverStar
    }
    
    public class StarView : MonoBehaviour
    {
        [SerializeField] private List<GameObject> starImage;

        public void Initialize(StarType type)
        {
            switch (type)
            {
                case StarType.GoldStar:
                    starImage[0].SetActive(true);
                    starImage[1].SetActive(false);
                    break;
                case StarType.SilverStar:
                    starImage[0].SetActive(false);
                    starImage[1].SetActive(true);
                    break;
            }
        }
    }
}