using System;
using TMPro;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
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