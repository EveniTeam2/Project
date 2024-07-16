using System;
using System.Collections.Generic;
using Unit.GameScene.Module;
using Unit.GameScene.Stages.Creatures.FSM.ActOnInput;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.GameSceneManagers.Modules
{
    [Serializable]
    public class SceneDefaultSetting
    {
        [Header("Canvas Ref")] public CanvasController canvasController;
        [Header("Camera Ref")] public Camera mainCamera;
        [Header("BoardManager Prefabs")] public GameObject boardManagerPrefab;
        [Header("StageManager Prefabs")] public GameObject stageManagerPrefab;
        [Header("Player Input Actions")] public List<ActOnInput> actOnInputs;
        [Header("Player Spawn Position")] public Vector3 playerPosition;
    }
}