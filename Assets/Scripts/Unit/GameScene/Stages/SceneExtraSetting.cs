using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Characters.Unit.Character;
using Unit.GameScene.Stages.Creatures.FSM.ActOnInput;
using UnityEngine;

namespace Unit.GameScene.Stages
{
    // TODO : factory pattern을 사용하여 battle stage setting을 생성하여 넘겨줘야 한다.
    // TODO : 이전 씬에서 SceneExtraSetting 객체를 전달하는 방식으로 변경 예정
    [Serializable]
    public struct SceneExtraSetting
    {
        [Header("---- Limit Time ----")]
        [Header("제한 시간")] public float limitTime;
        
        [Space(5)]
        [Header("---- Player Settings ----")]
        // public AssetReference characterRef;
        [Header("플레이어 오브젝트")] public PlayerCreature characterRef;
        [Header("플레이어 스텟")] public CharacterStat characterStat;
        [Header("플레이어 입력 처리")] public List<ActOnInput> actOnInputs;
        [Header("플레이어 스폰 위치")] public Vector3 playerPosition;
        
        [Space(5)]
        [Header("---- Monster Settings ----")]
        [Header("몬스터 스폰 정보")] public MonsterSpawnData monsterSpawnData;
        //public AssetReference[] monstersRef;
        
        [Space(5)]
        [Header("Ground Position")] public float groundYPosition;

        [Space(5)]
        [Header("---- Block Settings ----")]
        [Header("블록 정보")] public List<BlockSo> blockInfos;
    }
}