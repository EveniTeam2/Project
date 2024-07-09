using UnityEngine;

namespace ScriptableObjects.Scripts.Blocks
{
    public enum BlockType
    {
        Normal,
        BuffHealth,
        BuffSpeed,
        Debuff
    }
    
    public class NewBlock : ScriptableObject
    {
        public BlockType type;
        public Color color;
        public string text;
    }
}