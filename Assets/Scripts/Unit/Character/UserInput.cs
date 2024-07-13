using ScriptableObjects.Scripts.Blocks;
using System;

namespace Unit.Character {
    public class UserInput {
        private readonly PlayerCharacter player;

        public UserInput(PlayerCharacter target) {
            this.player = target;
        }
        public void Input(NewBlock block, int count) {
            GetAction(block).Invoke(count);
        }
        private Action<int> GetAction(NewBlock block) {
            // TODO 인호님!!! 바꿔야되는 거 여기랑 요 아래!
            switch (block.type) {
                case BlockType.Idle:
                    return NormalAct;
                case BlockType.Run:
                    return BuffHealthAct;
                case BlockType.Walk:
                    return BuffSpeedAct;
                case BlockType.Hit:
                    return DebuffAct;
            }
            return null;
        }
        private void NormalAct(int count) {
            // Idle
            player.HFSM.TryChangeState("Idle");
        }
        private void BuffHealthAct(int count) {
            // Run
            player.HFSM.TryChangeState("Run");
        }
        private void BuffSpeedAct(int count) {
            // Attack
            player.HFSM.TryChangeState("Attack1");
        }
        private void DebuffAct(int count) {
            // Hit
            player.HFSM.TryChangeState("Hit");
        }
    }
}