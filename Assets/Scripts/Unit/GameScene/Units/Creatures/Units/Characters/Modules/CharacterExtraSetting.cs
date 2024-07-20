using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Modules
{
    [Serializable]
    public class CharacterExtraSetting
    {
        [Header("캐릭터 스킬 프리셋 정보")] public List<string> characterSkillPresets;
    }
}