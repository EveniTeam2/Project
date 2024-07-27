using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts.Creature.Data.KnightData
{
    [CreateAssetMenu(fileName = nameof(KnightDataSo), menuName = nameof(CreatureDataSo) + "/" + nameof(CharacterDataSo) + "/" + nameof(KnightDataSo))]
    public class KnightDataSo : CharacterDataSo
    {
        [Header("기사 스킬 종류")]
        public List<KnightSkillData> skillData;
    }
}