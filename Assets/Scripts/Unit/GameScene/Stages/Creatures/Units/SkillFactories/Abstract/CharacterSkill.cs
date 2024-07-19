using System;
using Unit.GameScene.Stages.Creatures.Interfaces;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Characters.Enums;
using Unit.GameScene.Stages.Creatures.Units.Characters.Modules;
using Unit.GameScene.Stages.Creatures.Units.FSM;
using Unit.GameScene.Stages.Creatures.Units.SkillFactories.Interfaces;

namespace Unit.GameScene.Stages.Creatures.Units.SkillFactories.Abstract
{
    public abstract class CharacterSkill : Skill
    {
        protected readonly CharacterServiceProvider CharacterServiceProvider;

        protected CharacterSkill(CharacterServiceProvider characterServiceProvider)
        {
            CharacterServiceProvider = characterServiceProvider;
        }
    }
}