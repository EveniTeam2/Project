using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using ScriptableObjects.Scripts.Creature.Data;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Data.CharacterDatas;
using Unit.GameScene.Units.Creatures.Enums;
using UnityEngine;
using UnityEngine.Serialization;

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

        [Space(5), Header("---- Player Setting ----")]
        // TODO : 이전 씬에서 캐릭터를 선택하면 현재 씬에서 팩토리를 통해 스킬과 스탯을 생성하도록 해야 함
        public CharacterClassType characterClassType;
        public List<CreatureDataSo> characterData;
        // public AssetReference characterRef;

        [Space(5), Header("---- Monster Setting ----")]
        public MonsterSpawnData monsterSpawnData;
        //public AssetReference[] monstersRef;
        
        [Space(5), Header("---- Block Settings ----")]
        public List<BlockModel> blockInfos;
        
        [Space(5), Header("---- Map Settings ----")]
        public GameObject mapPrefab;
    }
}