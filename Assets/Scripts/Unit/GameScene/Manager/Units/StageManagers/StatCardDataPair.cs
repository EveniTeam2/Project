using System;
using Unit.GameScene.Units.Creatures.Abstract;

namespace Unit.GameScene.Manager.Units.StageManagers
{
    [Serializable]
    public struct StatCardDataPair
    {
        public EStatType statType;
        public int[] value;
    }
}