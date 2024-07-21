using Unit.GameScene.Stages.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public interface IMonsterSkillAct
    {
        void Act(IBattleStat stat, RaycastHit2D target);
    }
}