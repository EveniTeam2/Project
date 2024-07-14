using UnityEngine;

namespace Unit.GameScene.Stages {
    [CreateAssetMenu(fileName = nameof(RandomDistanceSpawnDecider), menuName = "SO/" + nameof(RandomDistanceSpawnDecider))]
    public class RandomDistanceSpawnDecider : SpawnDecider {
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        public override bool CanExecute(MonsterSpawnManager manager) {
            var decision = manager.StageManager.Character.transform.position.x;
            if (decision > minDistance && decision < maxDistance) {
                return true;
            }
            return false;
        }

        public override bool Execute(MonsterSpawnManager manager, MonsterGroup group) {
            if (CanExecute(manager)) {
                var select = Random.Range(0, group.TotalWeight + 1);
                int weight = 0;
                foreach (var item in group.monsterSpawnGroups) {
                    weight += item.weight;
                    if (weight > select) {
                        manager.SpawnMonster(item);
                        return true;
                    }
                }
            }
            return false;
        }
    }


    public class RandomTimeSpawnDecider : SpawnDecider {
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;
        public override bool CanExecute(MonsterSpawnManager manager) {
            var decision = manager.StageManager.PlayTime;
            if (decision > minTime && decision < maxTime)
                return true;
            return false;
        }

        public override bool Execute(MonsterSpawnManager manager, MonsterGroup group) {
            if (CanExecute(manager)) {
                var select = Random.Range(0, group.TotalWeight + 1);
                int weight = 0;
                foreach (var item in group.monsterSpawnGroups) {
                    weight += item.weight;
                    if (weight > select) {
                        manager.SpawnMonster(item);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}