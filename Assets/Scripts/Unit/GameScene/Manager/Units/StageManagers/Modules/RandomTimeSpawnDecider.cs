using Unit.GameScene.Stages;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules
{
    [CreateAssetMenu(fileName = nameof(RandomTimeSpawnDecider), menuName = "SO/" + nameof(RandomTimeSpawnDecider))]
    public class RandomTimeSpawnDecider : SpawnDecider
    {
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;
        [SerializeField] private float intervalTime = 1f;
 

        public override IMonsterSpawnDecider GetMonsterSpawnDecider() {
            var ret = new RandomTimeMonsterSpawnDecider(minTime, maxTime, intervalTime);
            return ret;
        }

    }

    public class RandomTimeMonsterSpawnDecider : IMonsterSpawnDecider {
        private float minTime;
        private float maxTime;
        private float intervalTime;
        private int count;

        public RandomTimeMonsterSpawnDecider(float minTime, float maxTime, float intervalTime) {
            this.minTime = minTime;
            this.maxTime = maxTime;
            this.intervalTime = intervalTime;
            this.count = 0;
        }

        public void Initialize() {
            count = 0;
            Debug.Log($"Count => {count}");
        }
        public bool CanExecute(MonsterSpawnManager manager) {
            var decision = manager.StageManager.PlayTime;
            if (decision > minTime && decision < maxTime)
                return true;
            return false;
        }

        public bool Execute(MonsterSpawnManager manager, StageMonsterGroup group) {
            if (CanExecute(manager)) {
                if ((manager.StageManager.PlayTime - minTime) / intervalTime < count)
                    return true;

                var select = Random.Range(0, group.TotalWeight + 1);
                var weight = 0;
                foreach (var item in group.monsterSpawnGroups) {
                    weight += item.weight;
                    if (weight > select) {
                        manager.SpawnMonster(item);
                        ++count;
                        Debug.Log($"{item.monsterIndex[0]}/{item.monsterStatIndex[0]} Count => {count}");
                    }
                }
                return true;
            }
            else
                return false;
        }
    }
}