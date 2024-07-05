using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    /// <summary>
    /// CanvasController 클래스: 캔버스와 관련된 컴포넌트를 관리하고 UI 매니저에 등록합니다.
    /// </summary>
    public class CanvasController : MonoBehaviour
    {
        private Canvas cvsComp;
        private CanvasScaler canvasScaler;
        private GraphicRaycaster graphicRaycaster;

        private void Start()
        {
            gameObject.TryGetComponent(out cvsComp);
            gameObject.TryGetComponent(out canvasScaler);

            var isRaycast = gameObject.TryGetComponent(out graphicRaycaster);
            UIManager.Instance.RegisterCanvas(this, new CanvasOption(cvsComp.renderMode, isRaycast));
        }

        public Canvas GetCanvasComponent()
        {
            return cvsComp;
        }
    }
}