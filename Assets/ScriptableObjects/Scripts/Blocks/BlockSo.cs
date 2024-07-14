using System;
using Unit.Boards.Blocks.Enums;
using UnityEngine;

namespace ScriptableObjects.Scripts.Blocks
{
    public class BlockSo : ScriptableObject
    {
        public BlockType type;
        public Color color;
        public string text;
    }
}