using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [Serializable]
    public struct MonsterSkillStateInfoDTO
    {
        public AnimationParameterEnums skillParameter;
        public int skillValue;
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;
        public MonsterSkillActDTO skillAct;

        public MonsterSkillStateInfo GetInfo(
            Transform transform,
            StateMachine stateMachine,
            Dictionary<AnimationParameterEnums, int> animationParameterEnums,
            IMonsterFsmController fsmController)
        {
            return new MonsterSkillStateInfo(animationParameterEnums[skillParameter], skillValue, targetLayer, direction, distance, skillAct.GetSkillAct(transform, fsmController, stateMachine, animationParameterEnums));
        }
    }
}