using System;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    /// <summary>
    /// UIBase 클래스: 모든 UI의 기본 추상 클래스입니다.
    /// </summary>
    public abstract partial class UIBase<UI> : MonoBehaviour {
        public Vector3 originScale = Vector3.one;

        /// <summary>
        /// 폰트 크기를 설정하는 메서드
        /// </summary>
        /// <param name="fontScale">폰트 크기에 곱해질 크기</param>
        public abstract void SetFontScale(float fontScale);

        public event Action ActOnClose;
        public event Action ActOnDraw;

        [Header("이 UI 요소의 캔버스 설정을 저장합니다.")]
        [SerializeField] private CanvasOption canvasOption;

        /// <summary>
        /// 현재 CanvasOption을 반환합니다.
        /// </summary>
        /// <returns>현재 CanvasOption</returns>
        public virtual CanvasOption GetCanvasOption() {
            return canvasOption;
        }

        /// <summary>
        /// UI 요소의 크기를 설정합니다.
        /// </summary>
        /// <param name="scale">원래 크기에 곱해질 크기</param>
        public virtual void SetScale(float scale) {
            transform.localScale = originScale * scale;
        }

        /// <summary>
        /// UI 요소를 닫고 OnClose 이벤트를 트리거합니다.
        /// </summary>
        /// <returns>이 UI</returns>
        public abstract UI CloseUI();

        /// <summary>
        /// UI를 업데이트하는 메서드
        /// </summary>
        /// <returns>이 UI</returns>
        public abstract UI UpdateUI();

        /// <summary>
        /// UI를 그리는 메서드
        /// </summary>
        /// <returns>이 UI</returns>
        public abstract UI DrawUI();

        /// <summary>
        /// UI를 초기화하는 메서드
        /// </summary>
        /// <returns>이 UI</returns>
        public abstract UI InitUI();

        protected void CallActOnDraw() {
            ActOnDraw?.Invoke();
        }

        protected void CallActOnClose() {
            ActOnClose?.Invoke();
        }
    }

    /// <summary>
    /// 텍스트의 폰트 크기를 제어하는 구조체
    /// </summary>
    [Serializable]
    public struct TextPair {
        public TMP_Text text;
        public float fontSize;
    }
}