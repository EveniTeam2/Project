using System;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules
{
    [Serializable]
    public struct KnightSkillSetting
    {
        public Sprite skillIcon;
        public KnightSkillType skillType;
        public int skillIndex;
        public float skillEffectValue;
    }
}