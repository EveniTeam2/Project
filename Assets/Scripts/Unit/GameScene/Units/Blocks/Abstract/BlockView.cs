using System;
using ScriptableObjects.Scripts.Blocks;
using Unit.GameScene.Units.Blocks.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Unit.GameScene.Units.Blocks.Abstract
{
    public class BlockView : MonoBehaviour
    {
        public BlockType Type { get; protected set; }
        
        [SerializeField] protected Image blockBackground;
        [SerializeField] protected Image blockIcon; // TODO : 스킬 아이콘 넣을 곳
    }
}