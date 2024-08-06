using System;
using ScriptableObjects.Scripts.Creature.DTO;
using ScriptableObjects.Scripts.Creature.DTO.MonsterDTOs;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Interfaces;
using Unit.GameScene.Units.Creatures.Interfaces.SkillControllers;
using Unit.GameScene.Units.FSMs.Units.Monster.Structs;
using UnityEngine;

namespace Unit.GameScene.Units.FSMs.Units.Monster.States
{
    public class MonsterDeathState : MonsterBaseState
    {
        private readonly DeathStateInfo _deathStateInfo;
        private readonly SpriteRenderer _spriteRenderer;
        private float _dampVel;

        public MonsterDeathState(MonsterBaseStateInfo monsterBaseStateInfo, DeathStateInfo deathStateInfo, Func<StateType, bool> tryChangeState, SpriteRenderer spriteRenderer, IMonsterFsmController fsmController)
            : base(monsterBaseStateInfo, tryChangeState, fsmController)
        {
            _deathStateInfo = deathStateInfo;
            _spriteRenderer = spriteRenderer;
        }

        public override void Enter()
        {
            base.Enter();
            FsmController.SetTrigger(MonsterBaseStateInfo.StateParameter, Exit);
            _dampVel = 0f;
        }

        public override void Exit()
        {
            base.Exit();
            Color color = _spriteRenderer.color;
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0);
        }

        public override void Update()
        {
            base.Update();
            Color color = _spriteRenderer.color;

            if (!(color.a * color.a > 0)) return;
            
            color.a = Mathf.SmoothDamp(color.a, 0, ref _dampVel, _deathStateInfo.FadeTime);
            _spriteRenderer.color = color;
        }
    }
}