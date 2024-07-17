using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Boards.Blocks.Enums;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules.Unit.Character;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.FSM.ActOnInput;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Stages.Creatures.Units.Characters
{
    public class Character : Creature
    {
        public CharacterType characterType;
        
        public override Animator Animator => _animator;
        public override BattleSystem Battle => _battleSystem;
        public override HealthSystem Health => _healthSystem;
        public override MovementSystem Movement => _movementSystem;
        public override LinkedList<ModifyStatData> ModifiedStatData => _mods;
        
        [SerializeField] private StateDataDTO stateData;
        
        private readonly LinkedList<ModifyStatData> _mods = new();
        
        private Animator _animator;
        private BattleSystem _battleSystem;
        private HealthSystem _healthSystem;
        private CommandInput _commandInput;
        private MovementSystem _movementSystem;
        private Stat<CharacterStat> _stats;
        private Dictionary<AnimationParameterEnums, int> _characterAnimationParameter;

        private void Update()
        {
            HFSM?.Update(this);
            Movement?.Update();
        }

        private void FixedUpdate()
        {
            HFSM?.FixedUpdate(this);
            Movement?.FixedUpdate();
        }

        public void Initialize(StageManager manager, CharacterSetting characterSetting, float groundYPosition)
        {
            _animator = GetComponent<Animator>();
            _stats = new Stat<CharacterStat>(characterSetting.Stat);
            _battleSystem = new BattleSystem(manager, this, _stats);
            _healthSystem = new HealthSystem(this, _stats);
            _movementSystem = new MovementSystem(transform, _stats);
            _movementSystem.SetGroundPosition(groundYPosition);

            characterType = characterSetting.Type;

            HFSM = StateBuilder.BuildState(this, stateData);
            
            // TODO : After
            // _input = new UserInput(this, characterSetting.Type, characterSetting.CharacterSkillPresets);
        }

        public void Input(BlockType blockType, int count)
        {
            _commandInput.Input(blockType, count);
        }

        public override void PermanentModifyStat(EStatType statType, int value)
        {
            _mods.AddLast(new ModifyStatData(statType, value));
            ModifyStat(statType, value);
        }

        protected void ModifyStat(EStatType statType, int value)
        {
            var cur = _stats.Current;
            switch (statType)
            {
                case EStatType.None:
                    break;
                case EStatType.Health:
                    cur.health += value;
                    break;
                case EStatType.Attack:
                    cur.attack += value;
                    break;
                case EStatType.Speed:
                    cur.speed += value;
                    break;
            }

            _stats.SetCurrent(cur);
        }

        public override void TempModifyStat(EStatType statType, int value, float duration)
        {
            StartCoroutine(TempModifyStatCoroutine(statType, value, duration));
        }

        private IEnumerator TempModifyStatCoroutine(EStatType statType, int value, float duration)
        {
            var data = new TempModifyStatData(statType, value, duration);
            var node = _mods.AddLast(data);
            ModifyStat(statType, value);
            while (duration <= 0)
            {
                duration -= Time.deltaTime;
                data.Duration = duration;
                yield return null;
            }

            _mods.Remove(node);
            ModifyStat(statType, -value);
        }

        public override void ClearStat()
        {
            _mods.Clear();
            _stats.SetCurrent(_stats.Origin);
        }
    }
}