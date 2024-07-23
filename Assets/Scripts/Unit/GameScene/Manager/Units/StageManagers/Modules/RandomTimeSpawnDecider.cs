using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules
{
    [CreateAssetMenu(fileName = nameof(RandomTimeSpawnDecider), menuName = "SO/" + nameof(RandomTimeSpawnDecider))]
    public class RandomTimeSpawnDecider : SpawnDecider
    {
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;

        public override IMonsterSpawnDecider GetMonsterSpawnDecider(StageScore score) {
            return new RandomTimeMonsterSpawnDecider(minTime, maxTime, score);
        }
    }

    public class RandomTimeMonsterSpawnDecider : IMonsterSpawnDecider {
        private float minTime;
        private float maxTime;
        private readonly StageScore score;

        public RandomTimeMonsterSpawnDecider(float minTime, float maxTime, StageScore score) {
            this.minTime = minTime;
            this.maxTime = maxTime;
            this.score = score;
        }

        public void Initialize() {

        }
        public bool CanExecute() {
            var decision = score.PlayTime;
            if (decision > minTime && decision < maxTime)
                return true;
            return false;
        }

        //public bool Execute(StageMonsterGroup group) {
        //    if (CanExecute()) {
        //        if ((score.PlayTime - minTime) / intervalTime < count)
        //            return true;

        //        var select = Random.Range(0, group.TotalWeight + 1);
        //        var weight = 0;
        //        foreach (var item in group.monsterSpawnGroups) {
        //            weight += item.weight;
        //            if (weight > select) {
        //                manager.SpawnMonster(item);
        //                ++count;
        //                Debug.Log($"{item.monsterIndex[0]}/{item.monsterStatIndex[0]} Count => {count}");
        //            }
        //        }
        //        return true;
        //    }
        //    else
        //        return false;
        //}
    }
}