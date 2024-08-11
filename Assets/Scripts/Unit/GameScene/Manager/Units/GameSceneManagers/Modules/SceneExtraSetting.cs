using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using ScriptableObjects.Scripts.Cards;
using ScriptableObjects.Scripts.Creature.Data;
using ScriptableObjects.Scripts.Creature.Data.MonsterData;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.GameSceneManagers.Modules
{
    // TODO : factory pattern을 사용하여 battle stage setting을 생성하여 넘겨줘야 한다.
    // TODO : 이전 씬에서 SceneExtraSetting 객체를 전달하는 방식으로 변경 예정
    [Serializable]
    public class SceneExtraSetting
    {
        [Header("---- Camera Position ----"), Space(5)]
        public Vector3 cameraSpawnPosition;
        
        [Header("---- Player Spawn Position ----"), Space(5)]
        public Vector3 playerSpawnPosition;

        [Header("---- Player Setting ----"), Space(5)]
        // TODO : 이전 씬에서 캐릭터를 선택하면 현재 씬에서 팩토리를 통해 스킬과 스탯을 생성하도록 해야 함
        public CharacterType characterType;
        public List<CreatureDataSo> characterDataSos;
        // public AssetReference characterRef;

        [Header("---- Monster Setting ----"), Space(5)]
        public List<MonsterDataSo> monsterDataSoLists;
        //public AssetReference[] monstersRef;
        
        [Header("---- Block Settings ----"), Space(5)]
        public List<BlockModel> blockInfos;

        [Header("---- StatCard Settings ----"), Space(5)]
        public StatCardSo statCardSos;
        
        [Header("---- Map Settings ----")]
        public GameObject mapPrefab;
    }
}