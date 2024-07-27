using System.Collections.Generic;
using System.Linq;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;
using Unit.GameScene.Units.Creatures.Units.Characters.Modules;

namespace Unit.GameScene.Units.Creatures.Units.SkillFactories.Modules
{
    public class StatManager
    {
        public int GetMaxHp(int level) => GetStatData().CharacterMaxHp;
        public int GetMaxExp(int level) => GetStatData().CharacterMaxExp;
        public int GetSpeed(int level) => GetStatData().CharacterSpeed;
        public int GetMaxShield(int level) => GetStatData().CharacterMaxShield;
        public int GetDamage(int level) => GetStatData().CharacterDamage;
        public bool GetCardTrigger(int level) => GetStatData().CardTrigger == 1;
        
        private readonly CharacterClassType _type;
        private readonly Dictionary<CharacterClassType, CharacterStatData> _statDataDictionary;

        public StatManager(CharacterClassType type, List<CharacterStatData> statDataList)
        {
            _type = type;
            _statDataDictionary = new Dictionary<CharacterClassType, CharacterStatData>();
            
            foreach (var statData in statDataList.Where(statData => !_statDataDictionary.ContainsKey(statData.CharacterType)))
            {
                _statDataDictionary.Add(statData.CharacterType, statData);
            }
        }

        private CharacterStatData GetStatData()
        {
            if (_statDataDictionary.TryGetValue(_type, out var statData))
            {
                return statData;
            }

            throw new KeyNotFoundException($"Stat data for character type {_type} not found.");
        }
    }
}