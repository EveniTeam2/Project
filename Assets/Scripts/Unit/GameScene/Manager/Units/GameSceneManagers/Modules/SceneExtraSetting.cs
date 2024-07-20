using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using ScriptableObjects.Scripts.Creature.Settings;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Manager.Units.GameSceneManagers.Modules
{
    // TODO : factory pattern을 사용하여 battle stage setting을 생성하여 넘겨줘야 한다.
    // TODO : 이전 씬에서 SceneExtraSetting 객체를 전달하는 방식으로 변경 예정
    [Serializable]
    public class SceneExtraSetting
    {
        [Header("---- Limit Time Setting ----")]
        public float limitTime;

        [Space(5)]
        [Header("---- Player Setting ----")]
        // TODO : 임시로 잡아둔 세팅입니다, 이전 씬에서 해당 값들을 보내줘야 함
        [Header("캐릭터 기본 정보")] public CharacterDefaultSetting characterDefaultSetting;
        [Header("캐릭터 추가 정보")] public CharacterExtraSetting characterExtraSetting;
        // public AssetReference characterRef;

        [Space(5)] [Header("---- Monster Setting ----")]
        [Header("몬스터 스폰 정보")] public MonsterSpawnData monsterSpawnData;
        //public AssetReference[] monstersRef;
        
        [Space(5)]
        [Header("---- Block Settings ----")] public List<BlockModel> blockInfos;
        
        [Space(5)]
        [Header("---- Map Settings ----")] public GameObject mapPrefab;
    }
}