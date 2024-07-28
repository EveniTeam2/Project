using System;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Units.Creatures;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    public class CharacterStatCard : Card
    {
        private StatCardDataPair[] data;
        public StatCardDataPair currentData => data[LevelStartFrom1];

        public CharacterStatCard(CharacterStatCardData data) : base(data)
        {
            this.data = data.statCardDataPairs;
        }

        public override void Apply(IStage stage)
        {
            foreach (var pair in data)
            {
                stage.Character.PermanentModifyStat(pair.statType, pair.value[LevelStartFrom1 - 1]);
            }
            LevelUp();
        }

        public override void LevelUp()
        {
            ++levelStartFrom1;
            base.LevelUp();
        }
    }

    [CreateAssetMenu(fileName = nameof(CharacterStatCardData), menuName = "Card/" + nameof(CharacterStatCardData))]
    public class CharacterStatCardData : CardData
    {
        public StatCardDataPair[] statCardDataPairs;

        public override Card GetCard()
        {
            return new CharacterStatCard(this);
        }
    }

    [Serializable]
    public struct StatCardDataPair
    {
        public EStatType statType;
        public int[] value;
    }
}