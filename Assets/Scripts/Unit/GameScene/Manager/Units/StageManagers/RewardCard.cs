using Unit.GameScene.Manager.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class RewardCard : Card
    {
        private int[] rewards;
        private ECurrencyType currencyType;

        public RewardCard(RewardCardData cardData) : base(cardData)
        {
            rewards = cardData.rewards;
            currencyType = cardData.currencyType;
        }

        public override void Apply(IStage stage)
        {
            Debug.Log($"{currencyType}이 들어온다~");
        }
    }

}