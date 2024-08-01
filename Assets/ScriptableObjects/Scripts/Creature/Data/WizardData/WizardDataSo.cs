using System.Collections.Generic;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Data;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Data.WizardData
{
    [CreateAssetMenu(fileName = nameof(WizardDataSo), menuName = nameof(CreatureDataSo) + "/" + nameof(CharacterDataSo) + "/" + nameof(WizardDataSo))]
    public class WizardDataSo : CharacterDataSo
    {
        [Header("마녀 스킬 종류")]
        public List<WizardSkillData> skillData;
    }
}