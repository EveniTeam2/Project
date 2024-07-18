using System;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract
{
    public abstract class CharacterSkill : Skill, ICharacterSkill
    {
        public event Action<int> OnActivateSkill;

        public override void RegisterServiceProvider(ICreatureServiceProvider serviceProvider)
        {
            base.RegisterServiceProvider(serviceProvider);
            
            OnActivateSkill += HandleDefineSkill;
        }
        
        protected abstract void HandleDefineSkill(int combo);
        
        public void ActivateSkill(int combo)
        {
            OnActivateSkill?.Invoke(combo);
        }
    }
}