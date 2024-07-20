using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(SkillStateDTO), menuName = "State/" + nameof(SkillStateDTO))]
    public class MonsterSkillStateDTO : MonsterBaseStateDTO
    {
        [Header("Skill State Info")]
        [SerializeField] protected MonsterSkillStateInfoDTO skillInfoDTO;

        public override IState BuildMonster(Transform transform,
                                            BattleSystem battleSystem,
                                            HealthSystem healthSystem,
                                            MovementSystem movementSystem,
                                            Animator animator,
                                            StateMachine stateMachine,
                                            Dictionary<AnimationParameterEnums, int> animationParameterEnums,
                                            MonsterEventPublisher eventPublisher)
        {
            return new MonsterSkillState(skillInfoDTO.GetInfo(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine, animationParameterEnums),
                                         monsterBaseStateInfoDTO.GetInfo(animationParameterEnums),
                                         monsterBaseStateInfoDTO.GetBaseInfo(animationParameterEnums),
                                         stateMachine.TryChangeState,
                                         battleSystem,
                                         animator,
                                         eventPublisher);
        }
    }

    [Serializable]
    public struct MonsterSkillStateInfoDTO
    {
        public AnimationParameterEnums skillParameter;
        public float skillValue;
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public MonsterSkillActDTO skillAct;
        public MonsterSkillStateInfo GetInfo(Transform transform,
                                             BattleSystem battleSystem,
                                             HealthSystem healthSystem,
                                             MovementSystem movementSystem,
                                             Animator animator,
                                             StateMachine stateMachine,
                                             Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new MonsterSkillStateInfo(animationParameterEnums[skillParameter],
                                             skillValue,
                                             targetLayer,
                                             direction,
                                             distance,
                                             skillAct.GetSkillAct(transform, battleSystem, healthSystem, movementSystem, animator, stateMachine, animationParameterEnums));
        }
    }

    public struct MonsterSkillStateInfo
    {
        public int skillParameter;
        public float skillValue;
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public IMonsterSkillAct skillAct;
        public MonsterSkillStateInfo(int skillParameter,
                                     float skillValue,
                                     LayerMask targetLayer,
                                     Vector2 direction,
                                     float distance,
                                     IMonsterSkillAct skillAct)
        {
            this.skillParameter = skillParameter;
            this.skillValue = skillValue;
            this.targetLayer = targetLayer;
            this.direction = direction;
            this.distance = distance;
            this.skillAct = skillAct;
        }
    }
}