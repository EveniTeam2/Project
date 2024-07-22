using System;
using ScriptableObjects.Scripts.Blocks;
using TMPro;
using Unit.GameScene.Units.Blocks.Abstract;
using Unit.GameScene.Units.Blocks.Units.MatchBlock.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Units.Blocks.Units.ComboBlock
{
    public class ComboBlockView : BlockView
    {
        [SerializeField] private TextMeshProUGUI comboCountText;
        
        public void Initialize(BlockType type, int comboCount, Sprite icon, Sprite background)
        {
            Type = type;

            blockBackground.sprite = background;
            blockIcon.sprite = icon;
            comboCountText.text = $"x {comboCount}";
        }
    }
}