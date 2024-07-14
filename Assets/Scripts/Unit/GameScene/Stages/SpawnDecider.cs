using UnityEngine;
using static Unit.GameScene.Stages.MonsterSpawnManager;

namespace Unit.GameScene.Stages {
    public abstract class SpawnDecider : ScriptableObject {
        public abstract bool CanExecute(MonsterSpawnManager manager);
        public abstract bool Execute(MonsterSpawnManager manager, MonsterGroup group);
    }
}