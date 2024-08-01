using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.FSMs.Units.Monster.States;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [CreateAssetMenu(fileName = nameof(MonsterDeathStateDTO), menuName = "State/Monster/" + nameof(MonsterDeathStateDTO))]
    public class MonsterDeathStateDTO : MonsterBaseStateDto
    {
        [Header("Death State Info")]
        [SerializeField] DeathStateInfoDTO deathStateInfoDTO;

        public override IState BuildState(Transform targetTransform, StateMachine stateMachine, Dictionary<AnimationParameterEnums, int> animationParameterHash, IMonsterFsmController fsmController, MonsterStatSystem monsterStatSystem)
        {
            var spriteRenderer = targetTransform.gameObject.GetComponent<SpriteRenderer>();
            return new MonsterDeathState(monsterBaseStateInfoDto.GetInfo(animationParameterHash), deathStateInfoDTO.GetInfo(), stateMachine.TryChangeState, spriteRenderer, fsmController);
        }
    }

    [Serializable]
    public struct DeathStateInfoDTO
    {
        public float fadeTime;

        public DeathStateInfo GetInfo()
        {
            return new DeathStateInfo(fadeTime);
        }
    }

    public struct DeathStateInfo
    {
        public readonly float FadeTime;

        public DeathStateInfo(float fadeTime)
        {
            FadeTime = fadeTime;
        }
    }
}