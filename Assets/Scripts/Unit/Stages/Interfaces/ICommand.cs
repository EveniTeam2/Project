namespace Unit.Stages.Interfaces
{
    public interface ICommand<T>
    {
        bool IsExecutable(T target);
        void Execute(T target);
    }
}