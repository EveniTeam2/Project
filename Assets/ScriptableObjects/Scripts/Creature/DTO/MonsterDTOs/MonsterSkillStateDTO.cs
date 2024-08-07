using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Monster.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterSkillStateDTO), menuName = "State/Monster/" + nameof(MonsterSkillStateDTO))]
    public class MonsterSkillStateDTO : MonsterBaseStateDto
    {
        [Header("Skill State Info")]
        [SerializeField] protected MonsterSkillStateInfoDTO[] skillInfoDto;

        public override IState BuildState(
            Transform targetTransform,
            StateMachine stateMachine,
            Dictionary<AnimationParameterEnums, int> animationParameterHash,
            IMonsterFsmController fsmController,
            MonsterStatSystem monsterStatSystem)
        {
            List<MonsterSkillStateInfo> skills = new List<MonsterSkillStateInfo>();
            foreach (var skillInfo in skillInfoDto) {
                skills.Add(skillInfo.GetInfo(targetTransform, stateMachine, animationParameterHash, fsmController));
            }

            return new MonsterSkillState(
                monsterBaseStateInfoDto.GetInfo(animationParameterHash),
                skills.ToArray(),
                stateMachine.TryChangeState, fsmController, monsterStatSystem);
        }
    }
}