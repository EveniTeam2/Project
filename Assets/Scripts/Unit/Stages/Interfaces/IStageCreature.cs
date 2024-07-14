using System.Collections.Generic;
using Unit.Stages.Creatures.Characters;
using Unit.Stages.Creatures.Monsters;

namespace Unit.Stages.Interfaces
{
    public interface IStageCreature
    {
        PlayerCreature Character { get; }
        List<MonsterCreature> Monsters { get; }
    }
}