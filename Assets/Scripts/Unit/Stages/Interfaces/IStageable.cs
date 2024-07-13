using System;

namespace Unit.Stages.Interfaces
{
    public interface IStageable
    {
        event Action<ICommand<IStageCreature>> OnSendCommand;
    }
}