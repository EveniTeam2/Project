using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using UnityEngine.UI;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Modules
{
    [Serializable]
    public struct KnightSkillSetting
    {
        public Image skillIcon;
        public KnightSkillType skillType;
        public int skillIndex;
    }
}