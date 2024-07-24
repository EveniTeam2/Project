using System;
using ScriptableObjects.Scripts.Creature.DTO;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.FSM
{
    public class DeathState : BaseState
    {
        private readonly DeathStateInfo _deathInfo;

        public DeathState(BaseStateInfo baseStateInfo, DeathStateInfo deathInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver)
            : base(baseStateInfo, tryChangeState, animatorEventReceiver)
        {
            _deathInfo = deathInfo;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorEventReceiver.SetTrigger(_baseStateInfo.stateParameter, null);
        }
        public override void Exit()
        {
            base.Exit();
        }
    }

    public class MonsterDeathState : MonsterBaseState
    {
        private readonly DeathStateInfo deathStateInfo;
        private readonly SpriteRenderer spriteRenderer;
        private float dampVel;

        public MonsterDeathState(MonsterBaseStateInfo monsterBaseStateInfo, DeathStateInfo deathStateInfo, Func<StateType, bool> tryChangeState, AnimatorEventReceiver animatorEventReceiver, SpriteRenderer spriteRenderer)
            : base(monsterBaseStateInfo, tryChangeState, animatorEventReceiver)
        {
            this.deathStateInfo = deathStateInfo;
            this.spriteRenderer = spriteRenderer;
        }

        public override void Enter()
        {
            base.Enter();
            animatorEventReceiver.SetTrigger(_monsterBaseStateInfo.stateParameter, ChangeToDefaultState);
        }

        public override void Exit()
        {
            base.Exit();
            var color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0);
        }

        public override void Update()
        {
            base.Update();
            var color = spriteRenderer.color;

            if (color.a * color.a > 0)
            {
                color.a = Mathf.SmoothDamp(color.a, 0, ref dampVel, deathStateInfo.fadeTime);
                spriteRenderer.color = color;
            }
        }
    }
}