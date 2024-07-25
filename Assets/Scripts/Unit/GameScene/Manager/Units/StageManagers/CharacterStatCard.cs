using System;
using Unit.GameScene.Units.Creatures;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class CharacterStatCard : Card
    {
        private StatCardDataPair[] data;

        public CharacterStatCard(StatCardData data) : base(data)
        {
            this.data = data.statCardDataPairs;
        }

        public override void Apply(CharacterServiceProvider character)
        {
            foreach (var pair in data)
            {
                character.ModifyStat(pair.statType, pair.value);
            }
        }
    }

    public class StatCardData : CardData
    {
        public StatCardDataPair[] statCardDataPairs;
    }

    [Serializable]
    public struct StatCardDataPair
    {
        public EStatType statType;
        public int value;
    }
}