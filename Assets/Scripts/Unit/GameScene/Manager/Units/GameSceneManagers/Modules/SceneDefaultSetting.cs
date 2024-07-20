using System;
using System.Collections.Generic;
using Unit.GameScene.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Manager.Units.GameSceneManagers.Modules
{
    [Serializable]
    public class SceneDefaultSetting
    {
        [Header("Canvas Ref"), Space(5)] public Canvas canvas;
        [Header("Camera Ref"), Space(5)] public Camera mainCamera;
        [Header("MatchBoardController Prefabs"), Space(5)] public GameObject matchBoardControllerPrefab;
        [Header("StageManager Prefabs"), Space(5)] public GameObject stageManagerPrefab;
        [Header("Player Spawn Position"), Space(5)] public Vector3 playerSpawnPosition;
    }
}