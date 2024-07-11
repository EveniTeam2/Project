using System;
using UnityEngine;

namespace ScriptableObjects.Scripts.Blocks
{
    [Serializable]
    public enum BlockType
    {
        Idle,
        Walk,
        Run,
        Hit
    }
    
    public class NewBlock : ScriptableObject
    {
        public BlockType type;
        public Color color;
        public string text;
    }
}