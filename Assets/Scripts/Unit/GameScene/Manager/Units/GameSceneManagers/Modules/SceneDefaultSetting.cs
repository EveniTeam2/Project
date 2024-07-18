using System;
using System.Collections.Generic;
using Unit.GameScene.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace Unit.GameScene.Manager.Units.GameSceneManagers.Modules
{
    [Serializable]
    public class SceneDefaultSetting
    {
        [Header("Canvas Ref"), Space(5)] public Canvas canvas;
        [Header("Camera Ref"), Space(5)] public Camera mainCamera;
        [Header("BoardManager Prefabs"), Space(5)] public GameObject boardManagerPrefab;
        [Header("StageManager Prefabs"), Space(5)] public GameObject stageManagerPrefab;
        [Header("Player Spawn Position"), Space(5)] public Vector3 playerSpawnPosition;
        [Header("Creature Animation Parameter"), Space(5)] public AnimationParameterEnums[] creatureAnimationParameter =
        {
            AnimationParameterEnums.Idle,
            AnimationParameterEnums.Run,
            AnimationParameterEnums.IsSprint,
            AnimationParameterEnums.SkillIndex,
            AnimationParameterEnums.Hit,
            AnimationParameterEnums.Die,
            AnimationParameterEnums.Skill
        };
    }
}