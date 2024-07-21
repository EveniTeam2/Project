using UnityEngine;
using UnityEngine.Serialization;

namespace Unit.GameScene.Module
{
    public class CanvasController : MonoBehaviour
    {
        public RectTransform MatchBlockPanel => matchBlockPanel;
        public RectTransform ComboBlockPanel => comboBlockPanel;
        public RectTransform ComboBlockEnter => comboBlockEnter;
        public RectTransform ComboBlockExit => comboBlockExit;
        
        [SerializeField] private RectTransform comboBlockPanel;
        [SerializeField] private RectTransform matchBlockPanel;
        [SerializeField] private RectTransform comboBlockEnter;
        [SerializeField] private RectTransform comboBlockExit;
    }
}
