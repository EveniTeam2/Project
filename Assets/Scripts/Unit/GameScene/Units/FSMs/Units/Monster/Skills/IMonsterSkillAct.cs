using System;
using Unit.GameScene.Units.Creatures.Module.Systems.MonsterSystems;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public interface IMonsterSkillAct
    {
        event Action OnExcute;

        void Act(MonsterStatSystem stat, RaycastHit2D target);
    }
}