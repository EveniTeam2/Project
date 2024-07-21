using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillAttackDTO), menuName = "State/Monster/" + nameof(MonsterSkillActDTO) + "/" + nameof(MonsterSkillAttackDTO))]
    public class MonsterSkillAttackDTO : MonsterSkillActDTO
    {
        [SerializeField] SkillAttackInfoDTO skillAttackInfoDTO;

        public override IMonsterSkillAct GetSkillAct(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa, AnimatorEventReceiver animatorEventReceiver)
        {
            return new MonsterSkillAttack(skillAttackInfoDTO.GetInfo(), ba);
        }
    }

    [Serializable]
    public struct SkillAttackInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;
        public SkillAttackInfo GetInfo()
        {
            return new SkillAttackInfo(targetLayer, direction, distance);
        }
    }

    public struct SkillAttackInfo
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;
        public SkillAttackInfo(LayerMask targetLayer, Vector2 direction, float distance)
        {
            this.targetLayer = targetLayer;
            this.direction = direction;
            this.distance = distance;
        }
    }
}