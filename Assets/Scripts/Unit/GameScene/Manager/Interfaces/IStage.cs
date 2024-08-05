using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units;

namespace Unit.GameScene.Manager.Interfaces
{
    public interface IStage
    {
        Character Character { get; }
        LinkedList<Monster> Monsters { get; }
    }
}