using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Monsters;

namespace Unit.GameScene.Manager.Interfaces
{
    public interface IStage
    {
        Character Character { get; }
        LinkedList<Monster> Monsters { get; }
    }
}