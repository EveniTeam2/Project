using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules {
    public abstract class SpawnDecider : ScriptableObject {

        public abstract IMonsterSpawnDecider GetMonsterSpawnDecider();
    }

    public interface IMonsterSpawnDecider {
        public void Initialize();
        public bool CanExecute(MonsterSpawnManager manager);
        public bool Execute(MonsterSpawnManager manager, StageMonsterGroup group);
    }
}