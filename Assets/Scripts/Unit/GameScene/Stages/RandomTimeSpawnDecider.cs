using UnityEngine;
using static UnityEditor.Progress;

namespace Unit.GameScene.Stages {
    [CreateAssetMenu(fileName = nameof(RandomTimeSpawnDecider), menuName = "SO/" + nameof(RandomTimeSpawnDecider))]
    public class RandomTimeSpawnDecider : SpawnDecider {
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;
        [SerializeField] private float intervalTime = 1f;
        private int count = 0;
        public override bool CanExecute(MonsterSpawnManager manager) {
            var decision = manager.StageManager.PlayTime;
            if (decision > minTime && decision < maxTime)
                return true;
            return false;
        }

        public override bool Execute(MonsterSpawnManager manager, MonsterGroup group) {
            if (CanExecute(manager)) {
                if ((manager.StageManager.PlayTime - minTime) / intervalTime < count)
                    return true;

                var select = Random.Range(0, group.TotalWeight + 1);
                int weight = 0;
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

        public override void Initialize() {
            count = 0;
            Debug.Log($"Count => {count}");
        }
    }
}