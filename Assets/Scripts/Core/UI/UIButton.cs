using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI {
    public class UIButton : UIBase {
        [SerializeField] protected Button btn;
        [SerializeField] protected TextPair text;
        public override UIBase InitUI() {
            return this;
        }

        public override UIBase DrawUI() {
            CallActOnDraw();
            return this;
        }

        public override UIBase CloseUI() {
            CallActOnClose();
            return this;
        }

        public override void SetFontScale(float fontScale) {
            text.text.fontSize = text.fontSize * fontScale;
        }

        public override UIBase UpdateUI() {
            return this;
        }
    }
}