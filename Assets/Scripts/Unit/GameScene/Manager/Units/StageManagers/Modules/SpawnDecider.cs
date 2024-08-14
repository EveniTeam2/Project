using UnityEngine;

namespace Unit.GameScene.Manager.Units.StageManagers.Modules {
    public abstract class SpawnDecider : ScriptableObject {

        public abstract IMonsterSpawnDecider GetMonsterSpawnDecider(StageScore score);
    }

    public interface IMonsterSpawnDecider {
        public void Initialize();
        public (bool,bool) CanExecute();
        //public bool Execute(StageMonsterGroup group);
    }
}