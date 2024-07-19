namespace Unit.GameScene.Manager.Units.StageManagers.Modules
{
    public class StageScore
    {
        private float score;
        private float playTime;
        private float distance;

        public float Score { get => score; }
        public float PlayTime { get => playTime; }
        public float Distance { get => distance; }

        public void SetStageScore(float time, float distance) {
            playTime = time;
            this.distance = distance;
        }
    }
}