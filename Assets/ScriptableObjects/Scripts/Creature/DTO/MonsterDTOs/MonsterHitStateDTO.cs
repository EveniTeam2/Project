using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Monster.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterHitStateDTO), menuName = "State/Monster/" + nameof(MonsterHitStateDTO))]
    public class MonsterHitStateDTO : MonsterBaseStateDto
    {
        public override IState BuildState(Transform targetTransform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterHash, IMonsterFsmController fsmController)
        {
            return new MonsterHitState(monsterBaseStateInfoDto.GetInfo(animationParameterHash), stateMachine.TryChangeState, fsmController);
        }
    }
}