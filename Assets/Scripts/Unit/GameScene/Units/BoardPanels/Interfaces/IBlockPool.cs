using Unit.GameScene.Units.Blocks.Abstract;

namespace Unit.GameScene.Units.BoardPanels.Interfaces
{
    /// <summary>
    ///     블록 풀링 인터페이스
    /// </summary>
    public interface IBlockPool
    {
        public BlockView Get();
        public void Release(BlockView blockView);
    }
}