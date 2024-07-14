using System;
using Unit.GameScene.Boards.Blocks.Enums;
using UnityEngine;

namespace ScriptableObjects.Scripts.Blocks
{
    [CreateAssetMenu(fileName = nameof(BlockSo), menuName = "Block/" + nameof(BlockSo))]
    public class BlockSo : ScriptableObject
    {
        public BlockType type;
        public Color color;
        public string text;
    }
}