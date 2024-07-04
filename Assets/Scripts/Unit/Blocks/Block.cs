using ScriptableObjects.Scripts.Blocks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.Blocks
{
    public abstract class Block : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private TextMeshPro text;
        private BlockData _blockData;

        public void Initialize(NewBlock newBlockInfo)
        {
            text.text = newBlockInfo.text;
            sprite.color = newBlockInfo.color;
        }

        protected void RegisterBlock()
        {
            
        }
    }
}