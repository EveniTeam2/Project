using Unit.Blocks;

namespace Unit.Boards.Interfaces
{
    public interface IBlockPool
    {
        Block Get();
        void Release(Block block);
    }
}