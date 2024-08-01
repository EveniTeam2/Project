using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules.Datas;
using UnityEngine;

namespace Unit.GameScene.Units.SkillFactories.Modules
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

                if (values.Length != 14)
                {
                    Debug.LogError($"Incorrect number of values in line: {line}");
                    continue;
                }
                
                if (!Enum.TryParse<CharacterClassType>(values[1], out var characterType))
                {
                    Debug.LogError($"Failed to parse CharacterClassType for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[2], out var skillIndex))
                {
                    Debug.LogError($"Failed to parse SkillIndex for line: {line}");
                    continue;
                }

                var skillName = values[3];
                var skillDescription = values[4];
                
                if (!int.TryParse(values[5], out var skillLevel))
                {
                    Debug.LogError($"Failed to parse skillLevel for line: {line}");
                    continue;
                }
                
                if (!int.TryParse(values[6], out var skillValue))
                {
                    Debug.LogError($"Failed to parse skillValue for line: {line}");
                    continue;
                }
                
                if (!float.TryParse(values[7], out var skillRange1))
                {
                    Debug.LogError($"Failed to parse skillRange1 for line: {line}");
                    continue;
                }
                
                if (!float.TryParse(values[8], out var skillRange2))
                {
                    Debug.LogError($"Failed to parse skillRange2 for line: {line}");
                    continue;
                }
                
                if (!Enum.TryParse<SkillType>(values[9], out var skillType))
                {
                    Debug.LogError($"Failed to parse SkillType for line: {line}");
                    continue;
                }
                
                if (!int.TryParse(values[10], out var isSingleTarget))
                {
                    Debug.LogError($"Failed to parse skillRange2 for line: {line}");
                    continue;
                }
                
                if (!Enum.TryParse<SkillRangeType>(values[11], out var skillRangeType))
                {
                    Debug.LogError($"Failed to parse skillRangeType for line: {line}");
                    continue;
                }
                
                if (!Enum.TryParse<SkillExtraEffectType>(values[12], out var skillExtraEffectType))
                {
                    Debug.LogError($"Failed to parse skillRangeType for line: {line}");
                    continue;
                }
                
                if (!float.TryParse(values[8], out var skillDuration))
                {
                    Debug.LogError($"Failed to parse skillDuration for line: {line}");
                    continue;
                }

                var skillData = new SkillData(characterType, skillIndex, skillName, skillDescription, skillLevel, skillValue, skillRange1, skillRange2, skillType, isSingleTarget, skillRangeType, skillExtraEffectType, skillDuration);
                
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

                if (!Enum.TryParse<CharacterClassType>(values[1], out var characterType))
                {
                    Debug.LogError($"Failed to parse CharacterClassType for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterLevel))
                {
                    Debug.LogError($"Failed to parse CharacterLevel for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterMaxExp))
                {
                    Debug.LogError($"Failed to parse CharacterMaxExp for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterMaxHp))
                {
                    Debug.LogError($"Failed to parse CharacterMaxHp for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterMaxShield))
                {
                    Debug.LogError($"Failed to parse CharacterMaxShield for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterDamage))
                {
                    Debug.LogError($"Failed to parse CharacterDamage for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[7], NumberStyles.Integer, CultureInfo.InvariantCulture, out var characterSpeed))
                {
                    Debug.LogError($"Failed to parse CharacterSpeed for line: {line}");
                    continue;
                }

                if (!int.TryParse(values[8], NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardTrigger))
                {
                    Debug.LogError($"Failed to parse CardTrigger for line: {line}");
                    continue;
                }

                var statData = new CharacterStatData(characterType, characterLevel, characterMaxExp, characterMaxHp, characterMaxShield, characterDamage, characterSpeed, cardTrigger);
                
                parsingResult.Add(statData);
            }

            return parsingResult;
        }
    }
}