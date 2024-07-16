using UnityEngine;

namespace Unit.GameScene.Module
{
    public class CanvasController : MonoBehaviour
    {
        public RectTransform BlockPanel => blockPanel;
        
        [SerializeField] private RectTransform blockPanel;
    }
}
