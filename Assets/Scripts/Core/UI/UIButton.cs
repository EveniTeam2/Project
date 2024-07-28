using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI {
    public class UIButton : UIBase<UIButton> {
        [SerializeField] protected Button btn;
        [SerializeField] protected TextPair text;
        public override UIButton InitUI() {
            return this;
        }

        public override UIButton DrawUI() {
            CallActOnDraw();
            return this;
        }

        public override UIButton CloseUI() {
            CallActOnClose();
            return this;
        }

        public override void SetFontScale(float fontScale) {
            text.text.fontSize = text.fontSize * fontScale;
        }

        public override UIButton UpdateUI() {
            return this;
        }
    }
}