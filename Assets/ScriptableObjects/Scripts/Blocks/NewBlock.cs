using UnityEngine;

namespace ScriptableObjects.Scripts.Blocks
{
    public enum BlockType
    {
        Idle,
        Run,
        FastRun,
        Hit
    }
    
    public class NewBlock : ScriptableObject
    {
        public BlockType type;
        public Color color;
        public string text;
    }
}