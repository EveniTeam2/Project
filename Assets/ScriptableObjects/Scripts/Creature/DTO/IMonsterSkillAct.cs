using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public interface IMonsterSkillAct
    {
        void Act(IBattleStat stat, RaycastHit2D target);
    }
}