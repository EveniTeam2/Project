using System;
using ScriptableObjects.Scripts.Blocks;
using Unit.Stages.Creatures.Characters;

namespace Unit.Stages.Creatures
{
    /// <summary>
    /// 사용자 입력을 처리하는 클래스입니다.
    /// </summary>
    public class UserInput
    {
        private readonly Character _character;

        public UserInput(Character target)
        {
            _character = target;
        }

        /// <summary>
        /// 입력을 처리합니다.
        /// </summary>
        public void Input(NewBlock block, int count)
        {
            GetAction(block).Invoke(count);
        }

        /// <summary>
        /// 블록에 따른 동작을 반환합니다.
        /// </summary>
        private Action<int> GetAction(NewBlock block)
        {
            // TODO 인호님!!! 바꿔야되는 거 여기랑 요 아래!
            switch (block.type)
            {
                case BlockType.Idle:
                    return NormalAct;
                case BlockType.Walk:
                    return BuffHealthAct;
                case BlockType.Run:
                    return BuffSpeedAct;
                case BlockType.Hit:
                    return DebuffAct;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 기본 동작을 수행합니다.
        /// </summary>
        private void NormalAct(int count)
        {
            _character.HFSM.TryChangeState("Idle");
        }

        /// <summary>
        /// 체력 버프 동작을 수행합니다.
        /// </summary>
        private void BuffHealthAct(int count)
        {
            _character.HFSM.TryChangeState("Run");
        }

        /// <summary>
        /// 속도 버프 동작을 수행합니다.
        /// </summary>
        private void BuffSpeedAct(int count)
        {
            _character.HFSM.TryChangeState("Attack1");
        }

        /// <summary>
        /// 디버프 동작을 수행합니다.
        /// </summary>
        private void DebuffAct(int count)
        {
            _character.HFSM.TryChangeState("Hit");
        }
    }
}