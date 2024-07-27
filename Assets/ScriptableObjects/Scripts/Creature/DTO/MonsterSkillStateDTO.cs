using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillStateDTO), menuName = "State/Monster/" + nameof(MonsterSkillStateDTO))]
    public class MonsterSkillStateDTO : MonsterBaseStateDTO
    {
        [Header("Skill State Info")]
        [SerializeField] protected MonsterSkillStateInfoDTO skillInfoDTO;

        public override IState BuildMonster(Transform transform,
                                            BattleSystem battleSystem,
                                            HealthSystem healthSystem,
                                            MovementSystem movementSystem,
                                            StateMachine stateMachine,
                                            AnimatorEventReceiver animatorEventReceiver,
                                            Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new MonsterSkillState(monsterBaseStateInfoDTO.GetInfo(animationParameterEnums),
                                         skillInfoDTO.GetInfo(transform, battleSystem, healthSystem, movementSystem, stateMachine, animationParameterEnums, animatorEventReceiver),
                                         stateMachine.TryChangeState,
                                         battleSystem, animatorEventReceiver);
        }
    }

    [Serializable]
    public struct MonsterSkillStateInfoDTO
    {
        public AnimationParameterEnums skillParameter;
        public int skillValue;
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public MonsterSkillActDTO skillAct;
        public MonsterSkillStateInfo GetInfo(Transform transform,
                                             BattleSystem battleSystem,
                                             HealthSystem healthSystem,
                                             MovementSystem movementSystem,
                                             StateMachine stateMachine,
                                             Dictionary<AnimationParameterEnums, int> animationParameterEnums,
                                             AnimatorEventReceiver animatorEventReceiver)
        {
            return new MonsterSkillStateInfo(animationParameterEnums[skillParameter],
                                             skillValue,
                                             targetLayer,
                                             direction,
                                             distance,
                                             skillAct.GetSkillAct(transform, battleSystem, healthSystem, movementSystem, stateMachine, animationParameterEnums, animatorEventReceiver));
        }
    }

    public struct MonsterSkillStateInfo
    {
        public int skillParameter;
        public int skillValue;
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;

        public IMonsterSkillAct skillAct;
        public MonsterSkillStateInfo(int skillParameter,
                                     int skillValue,
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