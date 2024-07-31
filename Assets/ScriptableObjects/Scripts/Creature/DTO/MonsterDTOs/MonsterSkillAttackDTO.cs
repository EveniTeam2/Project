using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillAttackDTO), menuName = "State/Monster/" + nameof(MonsterSkillActDTO) + "/" + nameof(MonsterSkillAttackDTO))]
    public class MonsterSkillAttackDTO : MonsterSkillActDTO
    {
        [SerializeField] SkillAttackInfoDTO skillAttackInfoDTO;
        public override IMonsterSkillAct GetSkillAct(
            Transform transform,
            IMonsterFsmController fsmController,
            StateMachine stateMachine,
            Dictionary<AnimationParameterEnums, int> animationParameterEnums)
        {
            return new MonsterSkillMeleeAttack(skillAttackInfoDTO.GetInfo(), fsmController);
        }
    }
}