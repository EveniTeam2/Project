using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.CharacterDTOs;
using Unit.GameScene.Module;
using Unit.GameScene.Units.Blocks.Enums;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules.Datas;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules.Systems;
using Unit.GameScene.Units.FSMs.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.Characters
{
    public class Character : Creature, ICharacterFsmController, ICharacterSkillController, ITakeDamage
    {
        public event Action OnPlayerLevelUp;

        [SerializeField] protected CharacterClassType characterClassType;
        [SerializeField] private CharacterStateMachineDto characterStateMachineDto;

        private readonly Queue<CommandPacket> _commandQueue = new();

        private CharacterData _characterData;

        private CharacterCommandSystem _characterCommandSystem;
        private CharacterSkillSystem _characterSkillSystem;

        private CharacterBattleSystem _characterBattleSystem;
        private CharacterHealthSystem _characterHealthSystem;
        private CharacterMovementSystem _characterMovementSystem;
        private CharacterStatSystem _characterStatSystem;

        private RectTransform _playerHpUI;

        public bool CheckEnemyInRange(LayerMask targetLayer, Vector2 direction, float range, out RaycastHit2D[] enemies) => _characterBattleSystem.CheckEnemyInRange(targetLayer, direction, range, out enemies);

        public void Initialize(CharacterData characterData, float groundYPosition, RectTransform playerHpUI, Dictionary<AnimationParameterEnums, int> animationParameter, Dictionary<BlockType, CharacterSkill> blockInfo)
        {
            var characterTransform = transform;
            _characterData = characterData;
            characterClassType = _characterData.CharacterDataSo.classType;
            _playerHpUI = playerHpUI;
            
            AnimatorSystem = GetComponent<AnimatorSystem>();
            AnimatorSystem.Initialize(animationParameter);
            _characterStatSystem = characterData.StatSystem;
            _characterSkillSystem = characterData.SkillSystem;
            _characterBattleSystem = new CharacterBattleSystem(_characterStatSystem, characterTransform);
            _characterHealthSystem = new CharacterHealthSystem(_characterStatSystem);
            _characterMovementSystem = new CharacterMovementSystem(_characterStatSystem, characterTransform, groundYPosition);
            _characterCommandSystem = new CharacterCommandSystem(blockInfo, _commandQueue);

            
            FsmSystem = StateBuilder.BuildCharacterStateMachine(characterStateMachineDto, this, AnimatorSystem, animationParameter);

            RegisterEventHandler();

            _characterData.RegisterCharacterServiceProvider(this);
            _characterStatSystem.InitializeStat();
        }

        protected void Update()
        {
            FsmSystem?.Update();
            _characterMovementSystem?.Update();
            _characterCommandSystem?.Update();
        }

        protected void FixedUpdate()
        {
            FsmSystem?.FixedUpdate();
            _characterMovementSystem?.FixedUpdate();
        }

        protected override void RegisterEventHandler()
        {
            OnUpdateStat += _characterStatSystem.HandleUpdateStat;
            AnimatorSystem.OnAttack += _characterCommandSystem.ActivateSkillEffects;
            
            _characterStatSystem.RegisterHandleOnDeath(HandleOnDeath);
            _characterStatSystem.RegisterHandleOnHit(HandleOnHit);
            _characterStatSystem.RegisterHandleOnUpdateHpUI(UpdateHealthBarUI);
        }

        protected override void HandleOnHit()
        {
            var stateType = FsmSystem.GetCurrentStateType();

            if (stateType is StateType.Skill or StateType.Hit)
            {
                return;
            }

            FsmSystem.TryChangeState(StateType.Hit);
            _characterMovementSystem.SetImpact(1);
        }

        protected override void HandleOnDeath()
        {
            FsmSystem.TryChangeState(StateType.Die);
        }
        
        protected override void UpdateHealthBarUI(int currentHp, int maxHp)
        {
            Debug.Log($"currentHp {currentHp} / maxHp {maxHp}");
            // 계산된 체력 비율
            float healthRatio = (float)currentHp / maxHp;
    
            // 새로운 localScale 값 계산
            var newScale = new Vector3(healthRatio, _playerHpUI.localScale.y, _playerHpUI.localScale.z);
    
            // 체력 바의 스케일을 업데이트
            _playerHpUI.localScale = newScale;
        }

        public void ToggleMovement(bool setRunning)
        {
            _characterMovementSystem.SetRun(setRunning);
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
            _characterHealthSystem.TakeHeal(value);
        }

        public void SetReadyForInvokingCommand(bool isReady)
        {
            _characterCommandSystem.SetReadyForInvokingCommand(isReady);
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

        public void AttackEnemy(int value, float range)
        {
            _characterBattleSystem.AttackEnemy(value, range);
        }

        public void Summon()
        {
            // TODO : 소환 스킬이 추가되면 고쳐야함
        }

        public void HandleReceiveCommand(CommandPacket command)
        {
            _commandQueue.Enqueue(command);
        }

        public void TakeDamage(int value)
        {
            _characterStatSystem.HandleUpdateStat(StatType.CurrentHp, value * -1);
        }

        public void RegisterHandleOnCommandDequeue(Action registerHandleOnCommandDequeue)
        {
            _characterCommandSystem.OnCommandDequeue += registerHandleOnCommandDequeue;
        }
    }
}