using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    [CreateAssetMenu(fileName = nameof(CharacterStatCardData), menuName = "Card/" + nameof(CharacterStatCardData))]
    public class CharacterStatCardData : CardData
    {
        public StatCardDataPair[] statCardDataPairs;

        public override Card GetCard()
        {
            return new CharacterStatCard(this);
        }
    }
}