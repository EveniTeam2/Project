using Unit.GameScene.Units.Creatures.Units.Monsters.Modules;
using Unit.GameScene.Units.Creatures.Units.Monsters.Modules.Systems;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public interface IMonsterSkillAct
    {
        void Act(MonsterStatSystem stat, RaycastHit2D target);
    }
}