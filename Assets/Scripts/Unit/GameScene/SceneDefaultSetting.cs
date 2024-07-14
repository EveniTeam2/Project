using System;
using UnityEngine;

namespace Unit.GameScene
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
        
        [Header("Map Prefabs")]
        public GameObject mapPrefab;
        
        [Header("Canvas")]
        public Canvas canvas;
        
        [Header("Block Spawn Position")]
        public RectTransform blockSpawnPos;
    }
}