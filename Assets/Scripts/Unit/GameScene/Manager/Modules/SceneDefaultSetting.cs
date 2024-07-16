using System;
using UnityEngine;

namespace Unit.GameScene.Manager.Modules
{
    [Serializable]
    public struct SceneDefaultSetting
    {
        [Header("Canvas Ref")] public Canvas canvas;
        [Header("Camera Ref")] public Camera mainCamera;
        [Header("BoardManager Prefabs")] public GameObject boardManagerPrefab;
        [Header("StageManager Prefabs")] public GameObject stageManagerPrefab;
    }
}