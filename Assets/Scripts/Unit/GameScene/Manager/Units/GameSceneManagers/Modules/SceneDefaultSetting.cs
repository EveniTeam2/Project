using System;
using System.Collections.Generic;
using TMPro;
using Unit.GameScene.Module;
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
        [Header("ComboBoardController Prefabs"), Space(5)] public GameObject comboBoardControllerPrefab;
        [Header("CardController Prefabs"), Space(5)] public GameObject cardControllerPrefab;
        [Header("StageManager Prefabs"), Space(5)] public GameObject stageManagerPrefab;
        [Header("PlayerHpPanel"), Space(5)] public RectTransform playerHpPanel;
        [Header("PlayerExpPanel"), Space(5)] public RectTransform playerExpPanel;
        [Header("PlayerLevelPanel"), Space(5)] public TextMeshProUGUI playerLevelPanel;
    }
}