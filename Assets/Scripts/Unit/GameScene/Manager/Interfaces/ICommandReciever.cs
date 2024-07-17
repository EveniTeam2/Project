namespace Unit.GameScene.Manager.Interfaces
{
    public interface ICommandReceiver<T>
    {
        void ReceiveCommand(ICommand<T> command);
        void UpdateCommand();
    }
}