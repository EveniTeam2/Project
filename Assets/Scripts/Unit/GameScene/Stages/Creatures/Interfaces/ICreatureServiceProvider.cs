using System;
using Unit.GameScene.Manager.Units.StageManagers;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Interfaces
{
    public interface ICreatureServiceProvider
    {
        public int GetSkillIndex(string skillName);
        public void AnimatorSetInteger(AnimationParameterEnums parameter, int value);
        public void AnimatorSetBool(AnimationParameterEnums parameter, bool value);
        public AnimatorStateInfo GetCurrentAnimatorStateInfo();
        public AnimatorStateInfo GetNextAnimatorStateInfo();
        public bool TryChangeState(StateType stateType);

        int Damage(int atk);
        void RegisterEvent(ECharacterEventType type, Action subscriber);
        void Run(bool isRun);
    }
}