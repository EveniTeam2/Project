using System;

namespace Unit.GameScene.Boards.Blocks.Enums
{
    [Serializable]
    public enum BlockType
    {
        Null = -1,
        Idle,
        Walk,
        Run,
        Hit
    }
}