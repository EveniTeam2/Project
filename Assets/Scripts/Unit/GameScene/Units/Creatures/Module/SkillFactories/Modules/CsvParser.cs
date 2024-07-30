using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Centaurs.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Knight.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Units.Wizard.Enums;
using UnityEngine;

namespace Unit.GameScene.Units.Creatures.Module.SkillFactories.Modules
{
    public static class CsvParser
    {
        public static List<SkillData> ParseCharacterSkillData(TextAsset skillCsv)
        {
            var parsingResult = new List<SkillData>();
            
            var reader = new StringReader(skillCsv.text);
            var headerCount = 2;

            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();

                if (headerCount > 0)
                {
                    headerCount--;
                    continue;
                }

                if (line == null) continue;
                var values = line.Split(',');

                if (values.Length != 9)
                {
                    Debug.LogError($"Incorrect number of values in line: {line}");
                    continue;
                }

                var skillId = values[0];
                
                if (!Enum.TryParse<CharacterClassType>(values[1], out var characterType))
                {
                    Debug.LogError($"Failed to parse CharacterClassType for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var skillIndex))
                {
                    Debug.LogError($"Failed to parse SkillIndex for line: {line}");
                    continue;
                }

                if (!Enum.TryParse<SkillEffectType>(values[3], out var skillType))
                {
                    Debug.LogError($"Failed to parse SkillType for line: {line}");
                    continue;
                }

                var skillNameEnum = characterType switch
                {
                    CharacterClassType.Knight => $"{(KnightSkillType) skillIndex}",
                    CharacterClassType.Wizard => $"{(WizardSkillType) skillIndex}",
                    CharacterClassType.Centaurs => $"{(CentaursSkillType) skillIndex}",
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                var skillName = values[4];
                var skillDescription = values[5];

                if (!float.TryParse(values[6], NumberStyles.Float, CultureInfo.InvariantCulture, out var skillRange))
                {
                    Debug.LogError($"Failed to parse SkillRange for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[7], NumberStyles.Integer, CultureInfo.InvariantCulture, out var skillEffectValue))
                {
                    Debug.LogError($"Failed to parse SkillEffectValue for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[8], NumberStyles.Integer, CultureInfo.InvariantCulture, out var skillLevel))
                {
                    Debug.LogError($"Failed to parse SkillLevel for line: {line}");
                    continue;
                }

                var skillData = new SkillData(skillId, characterType, skillIndex, skillType, skillNameEnum, skillName, skillDescription, skillRange, skillEffectValue, skillLevel);
                
                parsingResult.Add(skillData);
            }

            return parsingResult;
        }

        public static List<CharacterStatData> ParseCharacterStatData(TextAsset statCsv)
        {
            var parsingResult = new List<CharacterStatData>();
            
            var reader = new StringReader(statCsv.text);
            var headerCount = 2;

            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();

                if (headerCount > 0)
                {
                    headerCount--;
                    continue;
                }
                
                if (line == null) continue;
                var values = line.Split(',');

                if (values.Length != 9)
                {
                    Debug.LogError($"Incorrect number of values in line: {line}");
                    continue;
                }

                var statId = values[0];
                
                if (!Enum.TryParse<CharacterClassType>(values[1], out var characterType))
                {
                    Debug.LogError($"Failed to parse CharacterClassType for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterLevel))
                {
                    Debug.LogError($"Failed to parse CharacterLevel for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterMaxExp))
                {
                    Debug.LogError($"Failed to parse CharacterMaxExp for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterMaxHp))
                {
                    Debug.LogError($"Failed to parse CharacterMaxHp for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterMaxShield))
                {
                    Debug.LogError($"Failed to parse CharacterMaxShield for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterDamage))
                {
                    Debug.LogError($"Failed to parse CharacterDamage for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterSpeed))
                {
                    Debug.LogError($"Failed to parse CharacterSpeed for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[7], NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardTrigger))
                {
                    Debug.LogError($"Failed to parse CardTrigger for line: {line}");
                    continue;
                }

                var statData = new CharacterStatData(statId, characterType, characterLevel, characterMaxExp, characterMaxHp, characterMaxShield, characterDamage, characterSpeed, cardTrigger);
                
                parsingResult.Add(statData);
            }

            return parsingResult;
        }
    }
}