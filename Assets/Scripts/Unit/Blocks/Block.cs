using ScriptableObjects.Scripts.Blocks;
using System;
using TMPro;
using UnityEngine;

namespace Unit.Blocks
{
    public abstract class Block : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TextMeshPro text;

        // public Tuple<int, int> Index => _blockData.Index;
        // private BlockData _blockData;

        public BlockType Type { get; private set; }

        public void Initialize(NewBlock info)
        {
            text.text = info.text;
            sprite.color = info.color;
            Type = info.type;

            // _blockData.Index = new Tuple<int, int>(x, y);
        }

        protected void RegisterBlock()
        {
            
        }
    }
}