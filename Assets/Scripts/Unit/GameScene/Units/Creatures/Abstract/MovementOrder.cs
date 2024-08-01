using System;

namespace Unit.GameScene.Units.Creatures.Abstract
{
    public class MovementOrder
    {
        private Action<bool> act;
        private bool arg;

        public MovementOrder(Action<bool> act, bool arg)
        {
            this.act = act;
            this.arg = arg;
        }

        public void Execute()
        {
            act.Invoke(arg);
        }
    }
}