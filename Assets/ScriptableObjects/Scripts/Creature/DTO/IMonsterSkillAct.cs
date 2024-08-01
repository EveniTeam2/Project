using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public interface IMonsterSkillAct
    {
        void Act(RaycastHit2D target);
    }
}