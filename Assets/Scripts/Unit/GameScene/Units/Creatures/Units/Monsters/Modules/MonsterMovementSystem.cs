using System;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Systems;
using Unit.GameScene.Units.Creatures.Module.Systems.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Monsters.Modules
{
    public class MonsterMovementSystem : MovementSystem
    {
        public MonsterMovementSystem(Transform targetTransform, MonsterMovementStat stats, float ground) : base(
            targetTransform, stats, ground)
        {
        }

        public override void SetRun(bool isRun)
        {
            if (_impactDuration > 0)
                return;

            _wantToMove = isRun;
            if (isRun)
                _targetSpd = -1 * _stats.GetSpeed();
            else
                _targetSpd = 0;
        }

        public override void SetBackward(bool isBack)
        {
            _wantToMove = isBack;
            if (_wantToMove)
                _targetSpd = _stats.GetSpeed();
            else
                _targetSpd = 0;
        }

        public override void SpawnInit(IMovementStat movementStat)
        {
            _stats = movementStat;
        }
    }

    public class MonsterMovementStat : IMovementStat
    {
        protected Func<int> _getSpeed;

        public MonsterMovementStat(CreatureStat<MonsterStat> creatureStats)
        {
            _getSpeed = () => creatureStats.Current.Speed;
        }

        public int GetSpeed()
        {
            return _getSpeed();
        }
    }
}