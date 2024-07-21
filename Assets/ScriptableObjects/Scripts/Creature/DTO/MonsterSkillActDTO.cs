using System;
using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Module;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Units.Creatures.Module;
using UnityEngine;

namespace ScriptableObjects.Scripts.Creature.DTO
{
    public abstract class MonsterSkillActDTO : ScriptableObject
    {
        public abstract IMonsterSkillAct GetSkillAct(Transform tr, BattleSystem ba, HealthSystem he, MovementSystem mo, StateMachine st, Dictionary<AnimationParameterEnums, int> anPa, AnimatorEventReceiver animatorEventReceiver);
    }

}