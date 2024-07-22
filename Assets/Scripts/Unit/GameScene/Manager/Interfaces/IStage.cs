using System.Collections.Generic;
using Unit.GameScene.Units.Creatures.Units.Characters;
using Unit.GameScene.Units.Creatures.Units.Monsters;

namespace Unit.GameScene.Manager.Interfaces
{
    public interface IStage
    {
        Character Character { get; }
        LinkedList<Monster> Monsters { get; }
    }
}