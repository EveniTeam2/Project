using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Characters.Unit.Character;
using Unit.GameScene.Stages.Creatures.FSM.ActOnInput;
using UnityEngine;

namespace Unit.GameScene.Stages {
    // TODO factory pattern을 사용하여 battle stage setting을 생성하여 넘겨줘야 한다.
    [Serializable]
    public struct StageSetting {
        // public AssetReference characterRef;
        [Header("땅이라고 생각하는 y 포지션")]
        public float groundYPosition;
        [Header("플레이어 오브젝트")]
        public PlayerCreature characterRef;
        [Header("플레이어 스텟")]
        public CharacterStat characterStat;
        [Header("플레이어 입력 처리")]
        public List<ActOnInput> actOnInputs;
        [Header("플레이어 스폰 위치")]
        public Vector3 playerPosition;

        //public AssetReference[] monstersRef;

        [Header("몬스터 스폰 정보")]
        public MonsterSpawnManager.MonsterSpawnData monsterSpawnData;
    }
}