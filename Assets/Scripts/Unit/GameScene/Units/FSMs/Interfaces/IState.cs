namespace Unit.GameScene.Units.FSMs.Interfaces
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void Update();
    }
}