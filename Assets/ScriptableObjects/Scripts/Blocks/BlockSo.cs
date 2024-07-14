using System;
using TMPro;
using Unit.GameScene.Boards.Blocks.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects.Scripts.Blocks
{
    [CreateAssetMenu(fileName = nameof(BlockSo), menuName = "Block/" + nameof(BlockSo))]
    public class BlockSo : ScriptableObject
    {
        public BlockType type;
        public Sprite background;
        public string text;
    }
}