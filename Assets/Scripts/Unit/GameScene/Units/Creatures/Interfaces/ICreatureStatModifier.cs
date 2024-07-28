using Assets.Scripts.Unit.GameScene.Units.Creatures.Units;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Interfaces
{
    public interface ICreatureServiceProvider
    {
        public ICreatureStatModifier StatModifier { get; }
        public ICreatureBattle BattleSystem { get; }
        public ICreatureHeath HeathSystem { get; }
        public ICreatureMovement MovementSystem { get; }
    }

    public interface ICreatureStatModifier
    {
        public void PermanentModifyStat(EStatType statType, int value);
        public void TempModifyStat(EStatType statType, int value, float duration);
        public void ClearModifiedStat();
    }

    public interface ICreatureBattle
    {
        public bool CheckCollider(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collidee);
        public bool CheckCollider(LayerMask targetLayer, Vector2 startPosition, Vector2 direction, float distance, out RaycastHit2D[] collidee);
        public void Attack(RaycastHit2D col);
        public void Attack(RaycastHit2D col, IBattleEffect effect);
    }

    public interface ICreatureHeath
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
}