using Unit.GameScene.Stages;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules {
    [CreateAssetMenu(fileName = nameof(RandomDistanceSpawnDecider),
        menuName = "SO/" + nameof(RandomDistanceSpawnDecider))]
    public class RandomDistanceSpawnDecider : SpawnDecider {
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float intervalDistance = 1f;

        public override IMonsterSpawnDecider GetMonsterSpawnDecider() {
            var ret = new RandomDistanceMonsterSpawnDecider(minDistance, maxDistance, intervalDistance);
            return ret;
        }
    }

    public class RandomDistanceMonsterSpawnDecider : IMonsterSpawnDecider {
        private float minDistance;
        private float maxDistance;
        private float intervalDistance = 1f;
        private int count;

        public RandomDistanceMonsterSpawnDecider(float minDistance, float maxDistance, float intervalDistance) {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            this.intervalDistance = intervalDistance;
            this.count = 0;
        }

        public void Initialize() {
            count = 0;
        }

        public bool CanExecute(MonsterSpawnManager manager) {
            var decision = manager.StageManager.Distance;
            if (decision > minDistance && decision < maxDistance) {
                return true;
            }
            return false;
        }

        public bool Execute(MonsterSpawnManager manager, StageMonsterGroup group) {
            if (CanExecute(manager)) {
                if ((manager.StageManager.Distance - minDistance) / intervalDistance < count)
                    return true;

                var select = Random.Range(0, group.TotalWeight + 1);
                var weight = 0;
                foreach (var item in group.monsterSpawnGroups) {
                    weight += item.weight;
                    if (weight > select) {
                        manager.SpawnMonster(item);
                        ++count;
                        Debug.Log($"{item.monsterIndex[0]}/{item.monsterStatIndex[0]} => {count}");
                    }
                }
                return true;
            }
            else
                return false;
        }
    }
}