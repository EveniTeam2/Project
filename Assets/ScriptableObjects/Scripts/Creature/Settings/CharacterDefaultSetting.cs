using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Settings
{
    public class CharacterDefaultSetting : CreatureDefaultSetting
    {
        [Header("캐릭터 기본 타입"), Space(5)]
        public CharacterType characterType;
        
        [Header("캐릭터 기본 스탯"), Space(5)]
        public CharacterStat characterStat;
        
        [Header("애니메이션 파라미터"), Space(5)]
        public AnimationParameterEnums[] creatureAnimationParameter =
        {
            AnimationParameterEnums.Idle,
            AnimationParameterEnums.Run,
            AnimationParameterEnums.IsSprint,
            AnimationParameterEnums.SkillIndex,
            AnimationParameterEnums.Hit,
            AnimationParameterEnums.Die,
            AnimationParameterEnums.Skill
        };
    }
}