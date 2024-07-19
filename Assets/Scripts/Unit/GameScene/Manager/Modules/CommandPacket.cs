using System;
using Unit.GameScene.Manager.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using BlockType = Unit.GameScene.Boards.Blocks.Enums.BlockType;

namespace Unit.GameScene.Manager.Modules
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