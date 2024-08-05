using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public interface IMonsterSkillAct
    {
        void Act(MonsterStatSystem stat, RaycastHit2D target);
    }
}