namespace Unit.GameScene.Manager.Interfaces
{
    public interface ICommandReceiver<T>
    {
        void Received(ICommand<T> command);
        void UpdateCommand();
    }
}