using System;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Units.Creatures.Data.CharacterDatas
{
    [Serializable]
    public struct CharacterStatData
    {
        public CharacterType CharacterType { get; }
        public int CharacterLevel { get; }
        public int CharacterMaxExp{ get; }
        public int CharacterMaxHp{ get; }
        public int CharacterMaxShield{ get; }
        public int CharacterDamage{ get; }
        public int CharacterSpeed{ get; }
        public int CardTrigger{ get; }
        
        public CharacterStatData(CharacterType characterType, int characterLevel, int characterMaxExp, int characterMaxHp, int characterMaxShield, int characterDamage, int characterSpeed, int cardTrigger)
        {
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