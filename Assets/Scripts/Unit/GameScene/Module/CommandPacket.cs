using System;
using BlockType = Unit.GameScene.Units.Blocks.Enums.BlockType;

namespace Unit.GameScene.Module
{
    [Serializable]
    public class CommandPacket
    {
        public BlockType BlockType { get; }
        public int ComboCount { get; }

        public CommandPacket(BlockType blockType, int count)
        {
            BlockType = blockType;
            ComboCount = count;
        }
    }
}