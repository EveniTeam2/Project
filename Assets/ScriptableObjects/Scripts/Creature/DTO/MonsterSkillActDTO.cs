using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public abstract class MonsterSkillActDTO : ScriptableObject
    {
        public abstract IMonsterSkillAct GetSkillAct(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa);
    }

    public class MonsterSkillAttackDTO : MonsterSkillActDTO
    {

        private LayerMask targetLayer;
        private Vector2 direction;
        private float distance;

        public override IMonsterSkillAct GetSkillAct(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, Animator an, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa)
        {
            return new MonsterSkillAttack(ba, targetLayer, direction, distance);
        }
    }

    public struct SkillAttackInfoDTO
    {

    }

    public struct SkillAttackInfo
    {
        public BattleSystem battleSystem;
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;
    }
}