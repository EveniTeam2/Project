using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Scripts.Creature.Data.KnightData;
using Unit.GameScene.Units.Creatures.Enums;
using Unit.GameScene.Units.SkillFactories.Modules;
using Unit.GameScene.Units.SkillFactories.Units.CharacterSkills.Units;

namespace Unit.GameScene.Units.SkillFactories.Units.CharacterSkillFactories
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
                var csvData = skillCsvData.Where(data => data.SkillIndex == (int) knightSkillData.knightSkillType).ToList();
                
                if (csvData.Count == 0) continue;

                var skillProduct = new CharacterSkill();

                skillProduct.Initialize(knightSkillData.SkillIcon, csvData);

                skills.TryAdd($"{knightSkillData.knightSkillType}", skillProduct);
            }

            return skills;
        }
    }
}