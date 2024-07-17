using System;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput {
    public abstract class BattleSystem {
        protected Transform _targetTransform;
        protected IBattleStat _stat;

        public BattleSystem(Transform targetTransform, IBattleStat stat) {
            _targetTransform = targetTransform;
            _stat = stat;
        }

        public bool CheckCollider(LayerMask targetLayer, Vector2 direction, float distance, out RaycastHit2D[] collidee) {
            var hits = Physics2D.RaycastAll(_targetTransform.position, direction, distance, targetLayer);
            collidee = hits;
            if (collidee.Length > 0)
                return true;
            return false;
        }

        public abstract void Attack(RaycastHit2D col);
    }

    public interface IBattleStat {
        int GetAttack();
    }
}