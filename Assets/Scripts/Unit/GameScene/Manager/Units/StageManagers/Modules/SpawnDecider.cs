using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules {
    public abstract class SpawnDecider : ScriptableObject {
        public abstract void Initialize();
        public abstract bool CanExecute(MonsterSpawnManager manager);
        public abstract bool Execute(MonsterSpawnManager manager, MonsterGroup group);
        public abstract SpawnDecider GetCopy();
    }
}