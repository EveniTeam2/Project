using System;
using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Module;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.FSM;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public abstract class MonsterSkillActDTO : ScriptableObject
    {
        public abstract IMonsterSkillAct GetSkillAct(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa, AnimatorEventReceiver animatorEventReceiver);
    }

}