using System;
using Unit.GameScene.Stages.Creatures;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.Actions
{
    [CreateAssetMenu(fileName = nameof(CheckColliderAndAttack),
        menuName = "State/Act/" + nameof(CheckColliderAndAttack))]
    public class CheckColliderAndAttack : ActionData
    {
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Vector2 direction;
        [SerializeField] private float _distance;
        [SerializeField] private int _attackMultiplier;

        public override IStateAction GetStateAction(Transform transform, BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator) {
            return new StateActionCheckColliderAndAttack(targetLayer, direction, _distance, _attackMultiplier, battleSystem);
        }
    }

    public class StateActionCheckColliderAndAttack : IStateAction {
        private LayerMask targetLayer;
        private Vector2 direction;
        private float _distance;
        private int _attackMultiplier;
        private readonly BattleSystem _battleSystem;

        public StateActionCheckColliderAndAttack(LayerMask targetLayer, Vector2 direction, float distance, int attackMultiplier, BattleSystem battleSystem) {
            this.targetLayer = targetLayer;
            this.direction = direction;
            _distance = distance;
            _attackMultiplier = attackMultiplier;
            this._battleSystem = battleSystem;
        }

        public void OnAct(StateType stateName, int parameterHash) {
            if (_battleSystem.CheckCollider(targetLayer, direction, _distance, out var colldier)) {
                foreach (var col in colldier)
                    _battleSystem.Attack(col);
            }
        }
    }
}