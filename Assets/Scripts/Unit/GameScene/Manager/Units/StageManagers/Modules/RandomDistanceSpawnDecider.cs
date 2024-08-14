using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules
{
    [CreateAssetMenu(fileName = nameof(RandomDistanceSpawnDecider),
        menuName = "SO/" + nameof(RandomDistanceSpawnDecider))]
    public class RandomDistanceSpawnDecider : SpawnDecider
    {
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float intervalDistance = 1f;

        public override IMonsterSpawnDecider GetMonsterSpawnDecider(StageScore score)
        {
            return new RandomDistanceMonsterSpawnDecider(minDistance, maxDistance, intervalDistance, score);
        }
    }

    public class RandomDistanceMonsterSpawnDecider : IMonsterSpawnDecider
    {
        private float minDistance;
        private float maxDistance;
        private float intervalDistance = 1f;
        private readonly StageScore score;
        private int count;

        public RandomDistanceMonsterSpawnDecider(float minDistance, float maxDistance, float intervalDistance, StageScore score)
        {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            this.intervalDistance = intervalDistance;
            this.score = score;
            this.count = 0;
        }

        public void Initialize()
        {
            count = 0;
        }

        public (bool, bool) CanExecute()
        {
            var decision = score.Distance;
            if (decision > minDistance && decision < maxDistance)
            {
                if (decision / intervalDistance > count)
                {
                    ++count;
                    return (true, true);
                }
                else
                    return (true, false);
            }
            return (false, false);
        }

        //public bool Execute(StageMonsterGroup group) {
        //    if (CanExecute()) {
        //        if ((scorer.Distance - minDistance) / intervalDistance < count)
        //            return true;

        //        var select = Random.Range(0, group.TotalWeight + 1);
        //        var weight = 0;
        //        foreach (var item in group.monsterSpawnGroups) {
        //            weight += item.weight;
        //            if (weight > select) {
        //                manager.SpawnMonster(item);
        //                ++count;
        //                Debug.Log($"{item.monsterIndex[0]}/{item.monsterStatIndex[0]} => {count}");
        //            }
        //        }
        //        return true;
        //    }
        //    else
        //        return false;
        //}
    }
}