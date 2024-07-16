using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Manager.Units.StageManagers.Modules;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Characters.Unit.Character;
using UnityEngine;

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
        // public AssetReference characterRef;
        [Header("플레이어 오브젝트")] public Character characterRef;
        [Header("플레이어 스텟")] public CharacterStat characterStat;

        [Space(5)] [Header("---- Monster Setting ----")]
        [Header("몬스터 스폰 정보")] public MonsterSpawnData monsterSpawnData;
        //public AssetReference[] monstersRef;
        
        [Space(5)]
        [Header("---- Block Settings ----")] public List<BlockSo> blockInfos;
        
        [Space(5)]
        [Header("---- Map Settings ----")] public GameObject mapPrefab;
    }
}