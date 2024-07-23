using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public class MonsterSkillAttack : IMonsterSkillAct
    {
        SkillAttackInfo _skillAttackInfo;
        private BattleSystem _battleSystem;

        public MonsterSkillAttack(SkillAttackInfo skillAttackInfo, BattleSystem battleSystem)
        {
            _skillAttackInfo = skillAttackInfo;
            _battleSystem = battleSystem;
        }

        public void Act(IBattleStat stat, RaycastHit2D target)
        {
            _battleSystem.Attack(target);
        }
    }
}