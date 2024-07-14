using Unit.Boards.Blocks;

namespace Unit.Boards.Interfaces
{
    /// <summary>
    /// 블록 풀링 인터페이스
    /// </summary>
    public interface IBlockPool
    {
        Block Get();
        void Release(Block block);
    }
}