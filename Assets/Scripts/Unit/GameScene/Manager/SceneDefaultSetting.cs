using System;
using UnityEngine;

namespace Unit.GameScene.Manager
{
    [Serializable]
    public struct SceneDefaultSetting
    {
        [Header("Camera Ref")]
        public Camera mainCamera;

        [Header("BoardManager Prefabs")]
        public GameObject boardManagerPrefab;

        [Header("StageManager Prefabs")]
        public GameObject stageManagerPrefab;
        
        [Header("Canvas")]
        public Canvas canvas;
        
        [Header("Block Spawn Position")]
        public RectTransform blockSpawnPos;
    }
}