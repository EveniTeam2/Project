using System;
using TMPro;
using Unit.GameScene.Boards.Blocks.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects.Scripts.Blocks
{
    [CreateAssetMenu(fileName = nameof(BlockModel), menuName = "Block/" + nameof(BlockModel))]
    public class BlockModel : ScriptableObject
    {
        public BlockType type;
        public Sprite background;
    }
}