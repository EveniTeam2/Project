using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters
{
    public class Character : Creature
    {
        [SerializeField] protected CharacterType characterType;
        [SerializeField] private StateDataDTO stateData;

        protected UserInput _input;
        protected Stat<CharacterStat> _stats;
        protected CharacterServiceProvider _characterServiceProvider;

        protected void Update()
        {
            _fsm?.Update();
            _movementSystem?.Update();
        }

        protected void FixedUpdate()
        {
            _fsm?.FixedUpdate();
            _movementSystem?.FixedUpdate();
        }

        public void Initialize(CharacterSetting characterSetting, float groundYPosition)
        {
            _animator = GetComponent<Animator>();

            _stats = new Stat<CharacterStat>(characterSetting.Stat);
            _battleSystem = new CharacterBattleSystem(gameObject.transform, new CharacterBattleStat(_stats));
            _healthSystem = new CharacterHealthSystem(new CharacterHealthStat(_stats));
            _movementSystem = new CharacterMovementSystem(transform, new CharacterMovementStat(_stats), groundYPosition);

            characterType = characterSetting.Type;

            _fsm = StateBuilder.BuildState(stateData, transform, _battleSystem, _healthSystem, _movementSystem, _animator);
            _characterServiceProvider = new CharacterServiceProvider(_battleSystem, _healthSystem, _movementSystem, _animator, _fsm);
            // TODO : After
            _input = new UserInput(this, characterSetting.Type, characterSetting.CharacterSkills);
        }

        public void Input(BlockType blockType, int count)
        {
            _input.Input(blockType, count);
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
        }

        public CharacterServiceProvider GetServiceProvider() {
            return _characterServiceProvider;
        }

        protected override void ModifyStat(EStatType statType, int value) {
            var cur = _stats.Current;
            switch (statType) {
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

    public class CharacterServiceProvider : ICreatureServiceProvider {
        private readonly BattleSystem _battleSystem;
        private readonly HealthSystem _healthSystem;
        private readonly MovementSystem _movementSystem;
        private readonly Animator _animator;
        private readonly StateMachine _fsm;
        public CharacterServiceProvider(BattleSystem battleSystem, HealthSystem healthSystem, MovementSystem movementSystem, Animator animator, StateMachine fsm) {
            _battleSystem = battleSystem;
            _healthSystem = healthSystem;
            _movementSystem = movementSystem;
            _animator = animator;
            _fsm = fsm;
        }

        public void AnimatorSetBool(int parameterHash, bool onoff) {
            _animator.SetBool(parameterHash, onoff);
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

        public void Run(bool isRun) {
            _movementSystem.SetRun(isRun);
        }

        public bool TryChangeState(StateType stateType) {
            return _fsm.TryChangeState(stateType);
        }
    }
}