using System;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    [Serializable]
    public struct SkillAttackInfoDTO
    {
        [Header("사용하지 않는 더미 데이터인 상태")]
        public LayerMask targetLayer;
        public Vector2 direction;
        public float distance;
        public SkillAttackInfo GetInfo()
        {
            return new SkillAttackInfo(targetLayer, direction, distance);
        }
    }
}