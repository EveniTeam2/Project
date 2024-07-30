using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Scripts.Creature.Data.KnightData;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules;
using Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills.Abstract;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Units.CharacterSkills
{
    public class KnightSkillFactory : SkillFactory
    {
        private readonly KnightDataSo _knightDataSo;
        
        public KnightSkillFactory(KnightDataSo knightDataSo)
        {
            _knightDataSo = knightDataSo;
        }

        public override Dictionary<string, CharacterSkill> CreateSkill(List<SkillData> skillCsvData)
        {
            var skills = new Dictionary<string, CharacterSkill>();

            foreach (var knightSkillData in _knightDataSo.skillData)
            {
                var targetSkillName = knightSkillData.skillName;
                var csvData = skillCsvData.Where(data => data.SkillIndex == (int) targetSkillName).ToList();

                switch (csvData[0].SkillType)
                {
                    case SkillType.Attack:
                        break;
                    case SkillType.Heal:
                        break;
                    case SkillType.BuffDamage:
                        break;
                    case SkillType.BuffShield:
                        break;
                }
                
                switch (targetSkillName)
                {
                    case KnightSkillType.LightAttack:
                        skills.TryAdd($"{KnightSkillType.LightAttack}", new KnightLightAttack(knightSkillData, csvData));
                        break;
                    case KnightSkillType.SwordBuff:
                        skills.TryAdd($"{KnightSkillType.SwordBuff}", new KnightSwordBuff(knightSkillData, csvData));
                        break;
                    case KnightSkillType.HolyHeal:
                        skills.TryAdd($"{KnightSkillType.HolyHeal}", new KnightHolyHeal(knightSkillData, csvData));
                        break;
                    case KnightSkillType.HolySlash:
                        skills.TryAdd($"{KnightSkillType.HolySlash}", new KnightHolySlash(knightSkillData, csvData));
                        break;
                }
            }

            return skills;
        }
    }
}