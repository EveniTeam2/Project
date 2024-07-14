using System.Collections.Generic;
using Unit.GameScene.Stages.Creatures.Characters;
using Unit.GameScene.Stages.Creatures.Monsters;

namespace Unit.GameScene.Stages.Interfaces
{
    public interface IStageCreature
    {
        PlayerCreature Character { get; }
        List<MonsterCreature> Monsters { get; }
    }
}