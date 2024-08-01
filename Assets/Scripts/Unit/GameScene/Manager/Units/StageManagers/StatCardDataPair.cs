using System;
using Unit.GameScene.Units.Creatures.Abstract;
using Unit.GameScene.Units.Creatures.Enums;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    [Serializable]
    public struct StatCardDataPair
    {
        public StatType statType;
        public int[] value;
    }
}