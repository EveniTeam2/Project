namespace Unit.Stages.Creatures.Interfaces
{
    public interface ICharacterController
    {
        /// <summary>
        /// 캐릭터를 이동시킵니다.
        /// </summary>
        void Move();

        /// <summary>
        /// 캐릭터를 점프시킵니다.
        /// </summary>
        void Jump();

        /// <summary>
        /// 캐릭터가 공격합니다.
        /// </summary>
        void Attack();
    }
}