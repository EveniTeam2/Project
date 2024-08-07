using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public struct SkillAttackInfo
    {
        public LayerMask TargetLayer;
        public Vector2 Direction;
        public float Distance;
        
        public SkillAttackInfo(LayerMask targetLayer, Vector2 direction, float distance)
        {
            TargetLayer = targetLayer;
            Direction = direction;
            Distance = distance;
        }
    }
}