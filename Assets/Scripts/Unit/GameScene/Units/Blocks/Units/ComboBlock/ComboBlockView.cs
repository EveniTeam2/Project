using System;
using ScriptableObjects.Scripts.Blocks;
using TMPro;
using Unit.GameScene.Units.Blocks.Abstract;
using UnityEngine;

namespace Unit.GameScene.Units.Blocks.Units.ComboBlock
{
    public class ComboBlockView : BlockView
    {
        [SerializeField] private TextMeshProUGUI comboText;
        
        public void Initialize(BlockModel info)
        {
            
        }
    }
}