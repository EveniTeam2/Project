using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Monster.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterRunStateDTO), menuName = "State/Monster/" + nameof(MonsterRunStateDTO))]
    public class MonsterRunStateDTO : MonsterBaseStateDto
    {
        [Header("Run State Info")]
        [SerializeField] MonsterRunStateInfoDTO monsterRunStateInfoDTO;

        public override IState BuildState(Transform targetTransform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterHash, IMonsterFsmController fsmController)
        {
            return new MonsterRunState(monsterBaseStateInfoDto.GetInfo(animationParameterHash), monsterRunStateInfoDTO.GetInfo(), stateMachine.TryChangeState, fsmController);
        }
    }

    [Serializable]
    public struct MonsterRunStateInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public MonsterRunStateInfo GetInfo()
        {
            return new MonsterRunStateInfo(distance, direction, targetLayer);
        }
    }

    public struct MonsterRunStateInfo
    {
        public readonly float Distance;
        
        public LayerMask TargetLayer;
        public Vector2 Direction;

        public MonsterRunStateInfo(float distance, Vector2 direction, LayerMask targetLayer) : this()
        {
            Distance = distance;
            Direction = direction;
            TargetLayer = targetLayer;
        }
    }
}