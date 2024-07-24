using System;

namespace Unit.GameScene.Units.Creatures.Units.Characters.Modules
{
    namespace Unit.Character
    {
        [Serializable]
        public struct CharacterStat
        {
            public int Attack;
            public int Health;
            public int Speed;
            public float CoolTime;
        }
    }
}