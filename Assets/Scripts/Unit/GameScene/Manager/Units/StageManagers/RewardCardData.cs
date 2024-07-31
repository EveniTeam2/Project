using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    [CreateAssetMenu(fileName = nameof(RewardCardData), menuName = "Card/" + nameof(RewardCardData))]
    public class RewardCardData : CardData
    {
        public int[] rewards;
        public ECurrencyType currencyType;
        public override Card GetCard()
        {
            return new RewardCard(this);
        }
    }

}