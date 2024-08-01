using System;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [Serializable]
    public struct SkillAttackInfoDTO
    {
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;
        public SkillAttackInfo GetInfo()
        {
            return new SkillAttackInfo(targetLayer, direction, distance);
        }
    }
}