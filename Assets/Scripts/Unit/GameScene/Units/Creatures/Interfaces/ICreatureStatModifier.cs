using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces
{
    public interface ICreatureServiceProvider
    {
        public ICreatureStatModifier StatModifier { get; }
        public ICreatureBattle BattleSystem { get; }
        public ICreatureHealth HeathSystem { get; }
        public ICreatureMovement MovementSystem { get; }
        public StateMachine StateSystem { get; }
        AnimatorSystem AnimatorSystem { get; }
    }

    public interface ICharacterServiceProvider : ICreatureServiceProvider
    {
        // public ICharacterCommand CommandSystem { get; }
        // public ICharacterBattle BattleSystem { get; }
        // public ICharacterHealth HeathSystem { get; }
        // public ICharacterMovement MovementSystem { get; }
    }


    public interface ICreatureStatModifier
    {
        // public void PermanentModifyStat(EStatType statType, int value);
        // public void TempModifyStat(EStatType statType, int value, float duration);
        // public void ClearModifiedStat();
    }

    public interface ICreatureBattle
    {
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collidee);
        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 startPosition, Vector2 direction, float distance, out RaycastHit2D[] collidee);
        public void Attack(RaycastHit2D col);
        public void Attack(RaycastHit2D col, IBattleEffect effect);
        public void Attack(int damage, float range);
    }

    public interface ICharacterCommand
    {
        
    }
    
    public interface ICharacterBattle : ICreatureBattle
    {
        public int GetSkillIndex(string skillName);
        public int GetSkillValue(string skillName);
        public float GetSkillRange(string skillName);
    }

    public interface ICharacterHealth : ICreatureHealth
    {

    }

    public interface ICreatureHealth
    {
        public bool IsInvinsible();
        public void TakeDamage(int damage);
        public void TakeHeal(int heal);
    }

    public interface ICreatureMovement
    {
        public void SetRun(bool isRun);
        public void SetBackward(bool isBackward);
        public void Jump(float power);
        public void SetGroundPosition(float groundYPosition);
        public void SetImpact(Vector2 impact, float duration);
    }

    public interface ICharacterMovement : ICreatureMovement
    {

    }
}