namespace Unit.GameScene.Units.Creatures.Interfaces
{
    public interface IAnimationController
    {
        /// <summary>
        ///     달리는 애니메이션을 설정합니다.
        /// </summary>
        /// <param name="isRunning">달리는 상태인지 여부</param>
        void SetRunning(bool isRunning);

        /// <summary>
        ///     점프 애니메이션을 트리거합니다.
        /// </summary>
        void TriggerJump();

        /// <summary>
        ///     공격 애니메이션을 트리거합니다.
        /// </summary>
        void TriggerAttack();
    }
}