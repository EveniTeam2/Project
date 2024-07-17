using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Enums;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Modules
{
    [Serializable]
    public class KnightExtraSetting : CharacterExtraSetting
    {
        [Header("선택한 캐릭터 스킬 프리셋")] public KnightSkillType[] knightSkillPresets = new KnightSkillType[3];
    }
}