using UnityEngine;

namespace ScriptableObjects.Scripts.Blocks
{
    public enum BlockType
    {
        Normal,
        Buff,
        Debuff
    }
    
    public class NewBlock : ScriptableObject
    {
        public BlockType type;
        public Color color;
        public string text;
    }
}