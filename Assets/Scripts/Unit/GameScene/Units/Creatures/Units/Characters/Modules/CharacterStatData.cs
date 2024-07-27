using System;
using Unit.GameScene.Units.Creatures.Units.Characters.Enums;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    [Serializable]
    public struct CharacterStatData
    {
        public string StatId { get; }
        public CharacterClassType CharacterType { get; }
        public int CharacterLevel { get; }
        public int CharacterMaxExp{ get; }
        public int CharacterMaxHp{ get; }
        public int CharacterMaxShield{ get; }
        public int CharacterDamage{ get; }
        public int CharacterSpeed{ get; }
        public int CardTrigger{ get; }
        
        public CharacterStatData(string statId, CharacterClassType characterType, int characterLevel, int characterMaxExp, int characterMaxHp, int characterMaxShield, int characterDamage, int characterSpeed, int cardTrigger)
        {
            StatId = statId;
            CharacterType = characterType;
            CharacterLevel = characterLevel;
            CharacterMaxExp = characterMaxExp;
            CharacterMaxHp = characterMaxHp;
            CharacterMaxShield = characterMaxShield;
            CharacterDamage = characterDamage;
            CharacterSpeed = characterSpeed;
            CardTrigger = cardTrigger;
        }
    }
}