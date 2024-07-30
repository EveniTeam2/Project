using System;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules
{
    [Serializable]
    public class KnightSkillData : CharacterSkillData
    {
        public KnightSkillType skillName;
    }
}