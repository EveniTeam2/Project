namespace Unit.GameScene.Stages.Interfaces
{
    public interface ICommandReceiver<T>
    {
        void Received(ICommand<T> command);
        void UpdateCommand();
    }
}