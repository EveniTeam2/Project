using System;
using System.Collections.Generic;
using ScriptableObjects.Scripts.Creature.Data.KnightData;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Skills.Units;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Abstract;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Interfaces;
using Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Units.CharacterSkills.Units
{
    public class KnightSkillFactory : SkillFactory
    {
        private readonly KnightDataSo _knightDataSo;
        
        public KnightSkillFactory(KnightDataSo knightDataSo)
        {
            _knightDataSo = knightDataSo;
        }

        public override Dictionary<string, CharacterSkill> CreateSkill()
        {
            var skills = new Dictionary<string, CharacterSkill>();

            foreach (var knightSkillData in _knightDataSo.skillData)
            {
                switch (knightSkillData.skillName)
                {
                    case KnightSkillType.LightAttack:
                        skills.TryAdd($"{KnightSkillType.LightAttack}", new KnightLightAttack(knightSkillData));
                        break;
                    case KnightSkillType.SwordBuff:
                        skills.TryAdd($"{KnightSkillType.SwordBuff}", new KnightSwordBuff(knightSkillData));
                        break;
                    case KnightSkillType.HolyHeal:
                        skills.TryAdd($"{KnightSkillType.HolyHeal}", new KnightHolyHeal(knightSkillData));
                        break;
                    case KnightSkillType.HolySlash:
                        skills.TryAdd($"{KnightSkillType.HolySlash}", new KnightHolySlash(knightSkillData));
                        break;
                }
            }
            
            // foreach (var knightSkillData in _data.knightSkillData)
            // {
            //     switch (knightSkillData.skillName)
            //     {
            //         case KnightSkillType.LightAttack:
            //             ISkillCommand knightBaseAttack = new KnightBaseAttack();
            //             skills.Add(new CommandAction(knightBaseAttack));
            //             break;
            //         case KnightSkillType.SwordBuff:
            //             ISkillCommand knightSwordBuff = new KnightSwordBuff();
            //             skills.Add(new CommandAction(knightSwordBuff));
            //             break;
            //         case KnightSkillType.HolyHeal:
            //             ISkillCommand knightHolyHeal = new KnightHolyHeal();
            //             skills.Add(new CommandAction(knightHolyHeal));
            //             break;
            //         case KnightSkillType.HolySlash:
            //             ISkillCommand knightHolySlash = new KnightHolySlash();
            //             skills.Add(new CommandAction(knightHolySlash));
            //             break;
            //     }
            // }

            return skills;
        }
    }
}