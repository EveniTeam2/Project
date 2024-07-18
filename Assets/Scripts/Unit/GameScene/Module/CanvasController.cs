using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Module
{
    public class CanvasController : MonoBehaviour
    {
        public RectTransform ComboBlockPanel => comboBlockPanel;
        public RectTransform MatchBlockPanel => matchBlockPanel;
        
        [SerializeField] private RectTransform matchBlockPanel;
        [SerializeField] private RectTransform comboBlockPanel;
    }
}
