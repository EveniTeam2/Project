using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Monsters
{
    public class Monster : Creature
    {
        [SerializeField] private StateDataDTO stateData;

        protected Stat<MonsterStat> _stats;
        MonsterServiceProvider _monsterServiceProvider;

        private void Update()
        {
            _fsm?.Update();
            _movementSystem?.Update();
        }

        private void FixedUpdate()
        {
            _fsm?.FixedUpdate();
            _movementSystem?.FixedUpdate();
        }

        public void Initialize(MonsterStat stat, float groundYPosition)
        {
            _animator = gameObject.GetComponent<Animator>();

            _stats = new Stat<MonsterStat>(stat);
            _battleSystem = new MonsterBattleSystem(transform, new MonsterBattleStat(_stats));
            _healthSystem = new MonsterHealthSystem(new MonsterHealthStat(_stats));
            _movementSystem = new MonsterMovementSystem(transform, new MonsterMovementStat(_stats), groundYPosition);

            _fsm = StateBuilder.BuildState(stateData, transform, _battleSystem, _healthSystem, _movementSystem, _animator);

            _monsterServiceProvider = new MonsterServiceProvider(_battleSystem, _healthSystem, _movementSystem, _animator, _fsm);
        }

        public override void PermanentModifyStat(EStatType statType, int value)
        {
            _mods.AddLast(new ModifyStatData(statType, value));
            ModifyStat(statType, value);
        }


        public override void TempModifyStat(EStatType statType, int value, float duration)
        {
            StartCoroutine(TempModifyStatCoroutine(statType, value, duration));
        }

        public override void ClearModifiedStat()
        {
            _mods.Clear();
            _stats.SetCurrent(_stats.Origin);
            StopAllCoroutines();
        }

        public MonsterServiceProvider GetServiceProvider() {
            return _monsterServiceProvider;
        }

        protected override void ModifyStat(EStatType statType, int value)
        {
            var cur = _stats.Current;
            switch (statType)
            {
                case EStatType.None:
                    break;
                case EStatType.Health:
                    cur.Health += value;
                    break;
                case EStatType.Attack:
                    cur.Attack += value;
                    break;
                case EStatType.Speed:
                    cur.Speed += value;
                    break;
            }
            _stats.SetCurrent(cur);
        }
    }

    public class MonsterServiceProvider : ICreatureServiceProvider {
        private readonly BattleSystem _battleSystem;
        private readonly HealthSystem _healthSystem;
        private readonly MovementSystem _movementSystem;
        private readonly Animator _animator;
        private readonly StateMachine _fsm;

        public MonsterServiceProvider(BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine fsm) {
            _battleSystem = battleSystem;
            _healthSystem = healthSystem;
            _movementSystem = movementSystem;
            _animator = animator;
            _fsm = fsm;
        }

        public void AnimatorSetBool(int parameterHash, bool onoff) {
            _animator.SetBool(parameterHash, onoff);
        }

        public void Attack(RaycastHit2D hit) {
            _battleSystem.Attack(hit);
        }

        public bool CheckCollider(LayerMask targetLayer, Vector2 direction, float _distance, out RaycastHit2D[] collider) {
            return _battleSystem.CheckCollider(targetLayer, direction, _distance, out collider);
        }

        public int Damage(int atk) {
            _healthSystem.Damage(atk);
            return atk;
        }
        public AnimatorStateInfo GetCurrentAnimatorStateInfo() {
            return _animator.GetCurrentAnimatorStateInfo(0);
        }

        public AnimatorStateInfo GetNextAnimatorStateInfo() {
            return _animator.GetNextAnimatorStateInfo(0);
        }

        public void RegistEvent(ECharacterEventType type, Action subscriber) {
            switch (type) {
                case ECharacterEventType.Death:
                    _healthSystem.RegistOnDeathEvent(subscriber);
                    break;
                case ECharacterEventType.Damage:
                    _healthSystem.RegistOnDamageEvent(subscriber);
                    break;
            }
        }

        public void RegistStateEvent(StateType stateType, EStateEventType eventType, Action<StateType, int> subscriber) {
            if (_fsm.TryGetState(stateType, out IState state)) {
                switch (eventType) {
                    case EStateEventType.Enter:
                        state.OnEnter += subscriber;
                        break;
                    case EStateEventType.Exit:
                        state.OnExit += subscriber;
                        break;
                }
            }
        }

        public void Run(bool isRun) {
            _fsm.TryChangeState(Characters.Enums.StateType.Run);
            _movementSystem.SetRun(isRun);
        }

        public void UnregistStateEvent(StateType stateType, EStateEventType eventType, Action<StateType, int> subscriber) {
            if (_fsm.TryGetState(stateType, out IState state)) {
                switch (eventType) {
                    case EStateEventType.Enter:
                        state.OnEnter -= subscriber;
                        break;
                    case EStateEventType.Exit:
                        state.OnExit -= subscriber;
                        break;
                }
            }
        }
    }
}