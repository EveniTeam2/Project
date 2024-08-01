using Unit.GameScene.Units.Creatures.Interfaces.Battles;
using Unit.GameScene.Units.Creatures.Interfaces.Healths;
using Unit.GameScene.Units.Creatures.Interfaces.Movements;
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
}