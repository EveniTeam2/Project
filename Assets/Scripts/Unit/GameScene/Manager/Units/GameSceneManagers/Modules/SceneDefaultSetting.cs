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
        [Header("----- Canvas & Camera -----")]
        public Canvas canvas;
        public Camera mainCamera;
        
        [Header("----- DefaultUI -----")]
        public RectTransform comboBlockSpawnPanel;
        public RectTransform matchBlockSpawnPanel;

        [Header("----- PopUpUI -----")]
        public RectTransform blurPanel;
        public RectTransform cardPanel;
        public RectTransform cardSpawnPanel;
        
        [Header("----- CsvData -----")]
        public TextAsset characterDataCsv;
        public TextAsset characterSkillCsv;
        public TextAsset cardCsv;
        public TextAsset monsterDataCsv;
        public TextAsset monsterSkillCsv;
        
        [Header("----- PlayerUI -----")]
        public GameObject matchBoardControllerPrefab;
        public GameObject comboBoardControllerPrefab;
        public GameObject cardControllerPrefab;
        public GameObject stageManagerPrefab;
        
        [Header("----- PlayerUI -----")]
        public RectTransform playerHpHandler;
        public RectTransform playerExpHandler;
        public TextMeshProUGUI playerLevelHandler;
    }
}