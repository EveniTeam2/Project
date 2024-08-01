using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Monster.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterIdleStateDTO), menuName = "State/Monster/" + nameof(MonsterIdleStateDTO))]
    public class MonsterIdleStateDTO : MonsterBaseStateDto
    {
        [Header("Idle State Info")]
        [SerializeField]
        private MonsterIdleStateInfoDTO monsterIdleStateInfo;

        public override IState BuildState(Transform targetTransform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterHash, IMonsterFsmController fsmController)
        {
            return new MonsterIdleState(monsterBaseStateInfoDto.GetInfo(animationParameterHash), monsterIdleStateInfo.GetInfo(animationParameterHash), stateMachine.TryChangeState, fsmController);
        }
    }

    [Serializable]
    public struct MonsterIdleStateInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public MonsterIdleStateInfo GetInfo(Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new MonsterIdleStateInfo(targetLayer, direction, distance);
        }
    }

    public struct MonsterIdleStateInfo
    {
        public readonly float Distance;
        
        public LayerMask TargetLayer;
        public Vector2 Direction;

        public MonsterIdleStateInfo(LayerMask targetLayer, Vector2 direction, float distance)
        {
            TargetLayer = targetLayer;
            Direction = direction;
            Distance = distance;
        }
    }
}