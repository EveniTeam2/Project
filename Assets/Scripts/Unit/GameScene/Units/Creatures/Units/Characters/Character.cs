using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillController;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract;
using Unit.GameScene.Units.Creatures.Module.Systems;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters
{
    public class Character : Creature, ICharacterFsmController, ISkillController
    {
        public event Action OnLevelUp;
        public event Action OnCommandDequeue;
        
        [SerializeField] protected CharacterClassType characterClassType;
        [SerializeField] private CharacterStateMachineDto characterStateMachineDto;

        private readonly Queue<CommandPacket> _commandQueue = new();
        
        private CharacterData _characterData;
        private CreatureStat<CharacterStat> _creatureStats;
        
        public CharacterCommandSystem CommandSystem;
        public CharacterBattleSystem BattleSystem;
        public CharacterHealthSystem HealthSystem;
        public CharacterMovementSystem MovementSystem;
        public CharacterSkillSystem SkillSystem;
        public CharacterStatSystem StatSystem;

        public void HandleReceiveCommand(CommandPacket command)
        {
            _commandQueue.Enqueue(command);
        }

        public void Initialize(CharacterData characterData, float groundYPosition, Dictionary<AnimationParameterEnums, int> animationParameter, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            var characterTransform = transform;
            _characterData = characterData;
            characterClassType = _characterData.CharacterDataSo.classType;
            AnimatorSystem = GetComponent<AnimatorSystem>();
            AnimatorSystem.Initialize(animationParameter);
            _creatureStats = new CreatureStat<CharacterStat>(GetCharacterStat(1));

            StatSystem = characterData.StatSystem;
            SkillSystem = characterData.SkillSystem;
            BattleSystem = new CharacterBattleSystem(gameObject.transform, new CharacterBattleStat(_creatureStats, _characterData));
            HealthSystem = new CharacterHealthSystem(new CharacterHealthStat(_creatureStats));
            MovementSystem = new CharacterMovementSystem(characterTransform, new CharacterMovementStat(_creatureStats), groundYPosition);
            CommandSystem = new CharacterCommandSystem(blockInfo, _commandQueue, OnCommandDequeue);
            
            FsmSystem = StateBuilder.BuildCharacterStateMachine(characterStateMachineDto, this, AnimatorSystem, animationParameter);
            Mods = new LinkedList<ModifyStatData>();
            
            AnimatorSystem.OnAttack += CommandSystem.ActivateSkillEffects;
            
            _characterData.RegisterCharacterServiceProvider(this);
            HealthSystem.RegisterOnDamageEvent(CheckAndTransitToHit);
        }
        
        private CharacterStat GetCharacterStat(int currentLevel)
        {
            return new CharacterStat(
                _characterData.StatSystem.GetMaxHp(currentLevel),
                _characterData.StatSystem.GetMaxShield(currentLevel),
                _characterData.StatSystem.GetDamage(currentLevel),
                _characterData.StatSystem.GetSpeed(currentLevel),
                _characterData.StatSystem.GetCardTrigger(currentLevel),
                _characterData.StatSystem.GetMaxExp(currentLevel));
        }

        private void CheckAndTransitToHit()
        {
            var stateType = FsmSystem.GetCurrentStateType();
            
            if (stateType is StateType.Idle or StateType.Run)
            {
                FsmSystem.TryChangeState(StateType.Hit);
            }
        }

        protected void Update()
        {
            FsmSystem?.Update();
            MovementSystem?.Update();
            BattleSystem?.Update();
            CommandSystem?.Update();
        }
        
        protected void FixedUpdate()
        {
            FsmSystem?.FixedUpdate();
            MovementSystem?.FixedUpdate();
        }

        public override void PermanentModifyStat(EStatType statType, int value)
        {
            Mods.AddLast(new ModifyStatData(statType, value));
            ModifyStat(statType, value);
        }

        public override void TempModifyStat(EStatType statType, int value, float duration)
        {
            StartCoroutine(TempModifyStatCoroutine(statType, value, duration));
        }

        public override void ClearModifiedStat()
        {
            Mods.Clear();
            _creatureStats.SetCurrent(_creatureStats.Origin);
        }

        protected override void ModifyStat(EStatType statType, int value)
        {
            // TODO : 채이환
            var cur = _creatureStats.Current;
            
            switch (statType)
            {
                case EStatType.None:
                    break;
                case EStatType.Health:
                    cur.CurrentHp += value;
                    break;
                case EStatType.Attack:
                    cur.Damage += value;
                    break;
                case EStatType.Speed:
                    cur.Speed += value;
                    break;
            }

            _creatureStats.SetCurrent(cur);
        }

        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies)
        {
            return BattleSystem.CheckEnemyInRange(targetLayer, direction, range, out enemies);
        }

        public void ToggleMovement(bool setRunning)
        {
            MovementSystem.SetRun(setRunning);
        }

        public void SetBool(int parameter, bool value, Action action)
        {
            AnimatorSystem.SetBool(parameter, value, action);
        }

        public void SetTrigger(int parameter, Action action)
        {
            AnimatorSystem.SetTrigger(parameter, action);
        }

        public void SetInteger(int parameter, int value, Action action)
        {
            AnimatorSystem.SetInteger(parameter, value, action);
        }

        public void HealMySelf(int value)
        {
            // TODO : 힐
        }

        public void AttackEnemy(int value, float range)
        {
            BattleSystem.Attack(value, range);
        }

        public void SetReadyForInvokingCommand(bool isReady)
        {
            CommandSystem.SetReadyForInvokingCommand(isReady);
        }

        public void SetBoolOnAnimator(AnimationParameterEnums targetParameter, bool value, Action action)
        {
            AnimatorSystem.SetBool(targetParameter, value, action);
        }

        public void SetFloatOnAnimator(AnimationParameterEnums targetParameter, int value, Action action)
        {
            AnimatorSystem.SetFloat(targetParameter, value, action);
        }

        public void TryChangeState(StateType targetState)
        {
            FsmSystem.TryChangeState(targetState);
        }
    }
}