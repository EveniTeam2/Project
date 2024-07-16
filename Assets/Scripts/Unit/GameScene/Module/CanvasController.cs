using UnityEngine;

namespace Unit.GameScene.Module
{
    public class CanvasController : MonoBehaviour
    {
        public Canvas Canvas => canvas;
        public RectTransform BlockPanel => blockPanel;
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform blockPanel;
    }
}
