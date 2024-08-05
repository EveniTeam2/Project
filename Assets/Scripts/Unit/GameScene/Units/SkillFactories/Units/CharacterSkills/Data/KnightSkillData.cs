using System;
using Unit.GameScene.Units.Creatures.Data.CharacterDatas;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Enums;
using UnityEngine.Serialization;

namespace Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Data
{
    [Serializable]
    public class KnightSkillData : CharacterSkillData
    {
        public KnightSkillType knightSkillType;
    }
}