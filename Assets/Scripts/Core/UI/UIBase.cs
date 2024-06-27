using System;
using TMPro;
using UnityEngine;
namespace UI {
    /// <summary>
    /// Absract class for all UI
    /// </summary>
    public abstract class UIBase : MonoBehaviour {
        public Vector3 originScale = Vector3.one;
        /// <summary>
        /// Method to set the font scale
        /// </summary>
        /// <param name="fontScale">Scale that multiply to font size</param>
        public abstract void SetFontScale(float fontScale);
        /// <summary>
        /// Events to handle UI actions on close
        /// </summary>
        public event Action ActOnClose;
        /// <summary>
        /// Events to handle UI actions on draw
        /// </summary>
        public event Action ActOnDraw;
        [Header("Stores the canvas settings for this UI element")][SerializeField] CanvasOption canvasOption;
        /// <summary>
        /// Returns the current CanvasOption
        /// </summary>
        /// <returns>Current CanvasOption</returns>
        public virtual CanvasOption GetCanvasOption() {
            return canvasOption;
        }
        /// <summary>
        /// Sets the scale of the UI element
        /// </summary>
        /// <param name="scale">Scale that multiply to origin scale</param>
        public virtual void SetScale(float scale) {
            transform.localScale = originScale * scale;
        }
        /// <summary>
        /// Closes the UI element and triggers the OnClose event
        /// </summary>
        /// <returns>this UI</returns>
        public virtual UIBase CloseUI() {
            ActOnClose?.Invoke();
            return this;
        }
        /// <summary>
        /// Methods to update the UI
        /// </summary>
        /// <returns>this UI</returns>
        public abstract UIBase UpdateUI();
        /// <summary>
        /// Methods to draw the UI
        /// </summary>
        /// <returns>this UI</returns>
        public virtual UIBase DrawUI() {
            ActOnDraw?.Invoke();
            return this;
        }
        /// <summary>
        /// Methods to initialize the UI
        /// </summary>
        /// <returns>this UI</returns>
        public abstract UIBase InitUI();
    }
}

/// <summary>
/// Struct to control font size
/// </summary>
public struct TextPair {
    /// <summary>
    /// Reference to the text component
    /// </summary>
    public TMP_Text text;
    /// <summary>
    /// Font size for the text
    /// </summary>
    public float fontSize;
}