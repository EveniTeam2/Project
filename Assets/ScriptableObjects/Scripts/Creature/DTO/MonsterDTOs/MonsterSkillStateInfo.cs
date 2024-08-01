using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs
{
    public struct MonsterSkillStateInfo
    {
        public readonly int SkillParameter;
        public readonly int SkillValue;
        public readonly float Distance;
        public readonly IMonsterSkillAct SkillAct;

        public LayerMask TargetLayer;
        public Vector2 Direction;

        public MonsterSkillStateInfo(int skillParameter, int skillValue, LayerMask targetLayer, Vector2 direction, float distance, IMonsterSkillAct skillAct)
        {
            SkillParameter = skillParameter;
            SkillValue = skillValue;
            TargetLayer = targetLayer;
            Direction = direction;
            Distance = distance;
            SkillAct = skillAct;
        }
    }
}