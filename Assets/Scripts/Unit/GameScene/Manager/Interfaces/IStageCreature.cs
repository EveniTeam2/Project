using System.Collections.Generic;
using Unit.GameScene.Stages.Creautres.Characters;
using Unit.GameScene.Stages.Creautres.Monsters;

namespace Unit.GameScene.Manager.Interfaces
{
    public interface IStageCreature
    {
        PlayerCreature Character { get; }
        LinkedList<MonsterCreature> Monsters { get; }
    }
}