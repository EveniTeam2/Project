using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Units.Characters;
using Unit.GameScene.Stages.Creatures.Units.Monsters;
using Unit.GameScene.Units.Creatures.Units.Characters;

namespace Unit.GameScene.Manager.Interfaces
{
    public interface IStage
    {
        Character Character { get; }
        LinkedList<Monster> Monsters { get; }
    }
}