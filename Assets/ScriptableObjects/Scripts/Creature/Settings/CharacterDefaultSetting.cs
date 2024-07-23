using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules.Unit.Character;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Settings
{
    public class CharacterDefaultSetting : CreatureDefaultSetting
    {
        [Header("캐릭터 기본 타입"), Space(5)]
        public CharacterType characterType;
        
        [Header("캐릭터 기본 스탯"), Space(5)]
        public CharacterStat characterStat;
    }
}