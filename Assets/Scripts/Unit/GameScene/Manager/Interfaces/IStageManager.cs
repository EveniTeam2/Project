using System.Collections.Generic;
using Unit.GameScene.Manager.Units.GameSceneManagers.Modules;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Units.Creatures.Units;
using UnityEngine;

namespace Unit.GameScene.Manager.Interfaces
{
    public interface IStageManager
    {
        StageScore StageScore { get; }
        LinkedList<Monster> Monsters { get; }
        float PlayTime { get; }
        float Distance { get; }
        void Initialize(StageScore stageScore, Character character, SceneExtraSetting extraSetting, Camera cam);
        void Update();
    }
}