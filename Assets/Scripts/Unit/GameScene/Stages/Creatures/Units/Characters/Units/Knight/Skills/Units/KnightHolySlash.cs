using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;
using UnityEngine;

namespace Unit.GameScene.Stages.Creatures.Units.Characters.Units.Knight.Skills.Units
{
    public class KnightHolySlash : CharacterSkill, IChangeState
    {
        protected override void HandleDefineSkill(int combo)
        {
            Debug.Log($"{nameof(CharacterSkill)} - {nameof(KnightHolySlash)} x {combo}");
            ChangeState();
        }

        public void ChangeState()
        {
            ServiceProvider.TryChangeState(StateType.Skill);
        }
    }
}