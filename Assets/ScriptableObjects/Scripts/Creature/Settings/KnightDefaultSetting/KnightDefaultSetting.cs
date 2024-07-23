using System;
using System.Collections.Generic;
using Unit.GameScene.Manager.Interfaces;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ScriptableObjects.Scripts.Creature.Settings.KnightDefaultSetting
{
    [CreateAssetMenu(fileName = nameof(KnightDefaultSetting), menuName = nameof(CreatureDefaultSetting) + "/" + nameof(CharacterDefaultSetting) + "/" + nameof(KnightDefaultSetting))]
    public class KnightDefaultSetting : CharacterDefaultSetting
    {
        [Header("기사 스킬 종류")] public List<KnightSkillSetting> knightSkillSettings;
    }
}