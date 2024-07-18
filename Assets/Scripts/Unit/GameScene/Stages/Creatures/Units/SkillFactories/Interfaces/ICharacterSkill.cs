using System;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces
{
    public interface ICharacterSkill
    {
        public event Action<int> OnActivateSkill;

        public void ActivateSkill(int combo);
    }
}