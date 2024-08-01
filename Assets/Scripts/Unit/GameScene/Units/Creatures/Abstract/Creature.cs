using System;
using System.Collections;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.Creatures.Module.Animations;
using Unit.GameScene.Units.FSMs.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public abstract class Creature : MonoBehaviour
    {
        public Action<StatType, float> OnUpdateStat;
        
        public StateMachine FsmSystem;
        protected AnimatorSystem AnimatorSystem;

        protected abstract void RegisterEventHandler();
        protected abstract void HandleOnHit();
        protected abstract void HandleOnDeath();
    }
}