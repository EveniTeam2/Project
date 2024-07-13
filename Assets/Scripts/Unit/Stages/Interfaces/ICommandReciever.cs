namespace Unit.Stages.Interfaces
{
    public interface ICommandReceiver<T>
    {
        void Received(ICommand<T> command);
        void UpdateCommand();
    }
}