using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Module
{
    public class CanvasController : MonoBehaviour
    {
        public RectTransform MatchBlockPanel => matchBlockPanel;
        public RectTransform ComboBlockPanel => comboBlockPanel;
        
        [SerializeField] private RectTransform comboBlockPanel;
        [SerializeField] private RectTransform matchBlockPanel;
    }
}
