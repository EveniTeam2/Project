using UnityEngine;

namespace Unit.GameScene.Stages {
    [CreateAssetMenu(fileName = nameof(RandomDistanceSpawnDecider), menuName = "SO/" + nameof(RandomDistanceSpawnDecider))]
    public class RandomDistanceSpawnDecider : SpawnDecider {
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float intervalDistance = 1f;
        private int count = 0;

        public override bool CanExecute(MonsterSpawnManager manager) {
            var decision = manager.StageManager.Character.transform.position.x;
            if (decision > minDistance && decision < maxDistance) {
                return true;
            }
            return false;
        }

        public override bool Execute(MonsterSpawnManager manager, MonsterGroup group) {
            if (CanExecute(manager)) {
                if ((manager.StageManager.Distance - minDistance) / intervalDistance < count) {
                    return true;
                }
                ++count;

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