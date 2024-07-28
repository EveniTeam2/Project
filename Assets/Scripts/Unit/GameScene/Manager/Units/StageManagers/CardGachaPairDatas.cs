using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    [CreateAssetMenu(fileName = nameof(CardGachaPairDatas), menuName = "Card/" + nameof(CardGachaPairDatas))]
    public class CardGachaPairDatas : ScriptableObject
    {
        public CardGachaPairData[] datas;
        public CardGachaPair[] GetCardGachaPairs()
        {
            List<CardGachaPair> result = new List<CardGachaPair>();
            foreach (var data in datas)
            {
                CardGachaPair pair = new CardGachaPair(data.Card.GetCard(), data.Weight);
                result.Add(pair);
            }
            return result.ToArray();
        }
    }

    [Serializable]
    public struct CardGachaPairData
    {
        public CardData Card;
        public int Weight;
    }
}