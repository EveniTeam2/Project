using System;
using UnityEditor;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    [Serializable]
    public struct CharacterStat
    {
        public int CurrentHp { get; set; }
        public int MaxHp { get; set; }
        public int CurrentExp { get; set; }
        public int MaxExp { get; set;  }
        public int CurrentShield { get; set; }
        public int MaxShield { get; set;  }
        public int Damage { get; set; }
        public int Speed { get; set; }
        public bool CardTrigger { get; }
        public int CoolTime { get; set; }
        
        //TODO : 이환님 이쪽 스탯 가지고 오는 부분인데 기존에 작업하신 부분에 맞게 수정해주세요 (채이환)
        public CharacterStat(int maxHp, int maxShield, int damage, int speed, bool cardTrigger,  int maxExp)
        {
            CurrentHp = maxHp;
            MaxHp = maxHp;
            CurrentShield = maxShield;
            MaxShield = maxShield;
            CurrentExp = 0;
            MaxExp = maxExp;

            Damage = damage;
            Speed = speed;
            CardTrigger = cardTrigger;
            CoolTime = 0;
        }
    }
}