using System;
using UnityEngine.UI;

namespace ScriptableObjects.Scripts.Creature.Settings
{
    [Serializable]
    public struct CharacterSkillInfo
    {
        public Image skillIcon;
        public string skillType;
        public int skillIndex;
        public float skillValue;
    }
}