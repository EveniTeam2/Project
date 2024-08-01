using Unit.GameScene.Manager.Interfaces;

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
                stage.Character.OnUpdateStat?.Invoke(pair.statType, pair.value[LevelStartFrom1 - 1]);
            }
            LevelUp();
        }

        public override void LevelUp()
        {
            base.LevelUp();
        }
    }

}