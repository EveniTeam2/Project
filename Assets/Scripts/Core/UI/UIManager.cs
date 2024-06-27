using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI {
    /// <summary>
    /// Manages all UI operations such as loading, instantiating, etc.
    /// </summary>
    public class UIManager : Utils.UnitySingleton<UIManager> {
        /// <summary>
        /// List of UI asset references.
        /// </summary>
        private List<AssetReference> uiAssetRef;

        /// <summary>
        /// Map for type names to asset reference indices.
        /// </summary>
        private Dictionary<string, int> typeToAssetRefIndex;

        /// <summary>
        /// List of currently opened UIs.
        /// The first element is the oldest, and the last element is the most recently opened.
        /// </summary>
        private LinkedList<UIBase> openedUiList;

        /// <summary>
        /// Dictionary to keep track of UI instances by type name.
        /// </summary>
        private Dictionary<string, UIBase> uiDict;

        /// <summary>
        /// Map for UI to get canvas options.
        /// </summary>
        private Dictionary<CanvasOption, CanvasController> canvas;

        /// <summary>
        /// Initializes collections on UIManager initialization.
        /// </summary>
        /// <returns>The initialized UIManager instance.</returns>
        protected override UIManager Initialize() {
            openedUiList = new LinkedList<UIBase>();
            uiDict = new Dictionary<string, UIBase>();
            typeToAssetRefIndex = new Dictionary<string, int>();
            return this;
        }

        /// <summary>
        /// Sets the UI asset references for loading UI prefabs.
        /// </summary>
        /// <param name="uiAssetRef">List of UI asset references.</param>
        /// <returns>The UIManager instance.</returns>
        public UIManager SetData(List<AssetReference> uiAssetRef) {
            this.uiAssetRef = uiAssetRef;
            return this;
        }

        private void Start() {
            LoadUI<GameObject>();
        }

        /// <summary>
        /// Generic method to load UI assets and execute a callback upon completion.
        /// </summary>
        /// <typeparam name="T">The type of the UI component to load.</typeparam>
        /// <param name="onComplete">Callback to execute upon completion.</param>
        void LoadUI<T>(Action<T> onComplete = null) {
            for (int i = 0; i < uiAssetRef.Count; ++i) {
                int index = i;
                Utils.AddressableLoader.LoadAsset<GameObject>(uiAssetRef[i], obj => {
                    if (obj.TryGetComponent<T>(out T ui)) {
                        typeToAssetRefIndex.Add(ui.GetType().Name, index);
                        onComplete?.Invoke(ui);

#if UNITY_EDITOR
                        Debug.Log($"Loaded UI: {ui.GetType().Name}, Index: {index}");
#endif
                    }
                });
            }
        }

        /// <summary>
        /// Deploys the specified UI asset and executes a callback upon completion.
        /// </summary>
        /// <typeparam name="T">The type of the UI component to deploy.</typeparam>
        /// <param name="assetRef">The asset reference of the UI to deploy.</param>
        /// <param name="onComplete">Callback to execute upon completion.</param>
        void DeployUI<T>(AssetReference assetRef, Action<T> onComplete = null) where T : UIBase {
            Utils.AddressableLoader.DeployAsset(assetRef, Vector3.zero, Quaternion.identity, null, obj => {
                if (obj.TryGetComponent<T>(out T ui)) {
                    if (openedUiList == null) {
                        openedUiList = new LinkedList<UIBase>();
                    }
                    if (uiDict == null) {
                        uiDict = new Dictionary<string, UIBase>();
                    }

                    ui.ActOnDraw += () => openedUiList.AddLast(ui);
                    ui.ActOnClose += () => openedUiList.Remove(ui);
                    uiDict.Add(ui.GetType().Name, ui);
                    ui.InitUI();
                    if (obj.TryGetComponent<RectTransform>(out RectTransform uiRectTransform)) {
                        if (canvas == null) {
                            canvas = new Dictionary<CanvasOption, CanvasController>();
                        }
                        uiRectTransform.SetParent(canvas[ui.GetCanvasOption()].transform);
                        uiRectTransform.localEulerAngles = Vector3.zero;
                        uiRectTransform.localPosition = Vector3.zero;
                        uiRectTransform.localScale = Vector3.one;
                        uiRectTransform.sizeDelta = Vector2.zero;
                    }
                    onComplete?.Invoke(ui);

#if UNITY_EDITOR
                    Debug.Log($"Deployed UI: {ui.GetType().Name}");
#endif
                }
            });
        }

        /// <summary>
        /// Attempts to retrieve the specified UI by type name. If the UI is not loaded, it loads and deploys it.
        /// </summary>
        /// <typeparam name="T">The type of the UI component to retrieve.</typeparam>
        /// <param name="typeName">The type name of the UI to retrieve.</param>
        /// <param name="onLoadComplete">Callback to execute upon loading completion.</param>
        /// <returns>The retrieved UI component, or null if it is not yet loaded.</returns>
        public T TryGetUI<T>(string typeName, Action<T> onLoadComplete = null) where T : UIBase {
            if (uiDict != null && uiDict.TryGetValue(typeName, out var ui)) {
                onLoadComplete?.Invoke(ui as T);

#if UNITY_EDITOR
                Debug.Log($"UI found in dictionary: {typeName}");
#endif

                return ui as T;
            }

            if (typeToAssetRefIndex != null && typeToAssetRefIndex.TryGetValue(typeName, out int index)) {
                DeployUI<T>(uiAssetRef[index], onLoadComplete);

#if UNITY_EDITOR
                Debug.Log($"UI not found in dictionary, deploying: {typeName}");
#endif
            }
            else {
                LoadUI<T>(ui => DeployUI<T>(uiAssetRef[typeToAssetRefIndex[typeName]], onLoadComplete));

#if UNITY_EDITOR
                Debug.Log($"UI not found in dictionary and asset index map, loading and deploying: {typeName}");
#endif
            }

            return null;
        }

        /// <summary>
        /// Registers a canvas with the specified options.
        /// </summary>
        /// <param name="cv">The canvas controller to register.</param>
        /// <param name="opt">The canvas options.</param>
        public void RegisterCanvas(CanvasController cv, CanvasOption opt) {
            if (canvas == null) {
                canvas = new Dictionary<CanvasOption, CanvasController>();
            }

            if (!canvas.TryAdd(opt, cv)) {
                GameObject.Destroy(cv);

#if UNITY_EDITOR
                Debug.Log($"Canvas with option {opt} already exists. Destroying new canvas.");
#endif
            }
        }

        /// <summary>
        /// Attempts to retrieve the topmost UI.
        /// </summary>
        /// <returns>The topmost UI, or null if no UI is open.</returns>
        public UIBase TryGetTopUI() {
            if (openedUiList != null && openedUiList.Count > 0) {
#if UNITY_EDITOR
                Debug.Log($"Top UI: {openedUiList.Last.Value.GetType().Name}");
#endif
                return openedUiList.Last.Value;
            }

#if UNITY_EDITOR
            Debug.Log("No UI is currently open.");
#endif

            return null;
        }

        /// <summary>
        /// Updates the font scale and size scale for all opened UIs.
        /// </summary>
        /// <param name="fontScale">The font scale to set.</param>
        /// <param name="sizeScale">The size scale to set.</param>
        public void UpdateSize(float fontScale, float sizeScale) {
            if (openedUiList != null) {
                foreach (UIBase ui in openedUiList) {
                    ui.SetFontScale(fontScale);
                    ui.SetScale(sizeScale);

#if UNITY_EDITOR
                    Debug.Log($"Updated UI: {ui.GetType().Name} with font scale: {fontScale}, size scale: {sizeScale}");
#endif
                }
            }
        }

        /// <summary>
        /// Attempts to retrieve the canvas with the specified options.
        /// </summary>
        /// <param name="opt">The canvas options.</param>
        /// <returns>The retrieved canvas, or null if it does not exist.</returns>
        public Canvas TryGetCanvas(CanvasOption opt) {
            if (canvas != null && canvas.TryGetValue(opt, out var ui)) {
#if UNITY_EDITOR
                Debug.Log($"Canvas found for option: {opt}");
#endif
                return ui.GetCanvasComponent();
            }

#if UNITY_EDITOR
            Debug.Log($"Canvas not found for option: {opt}");
#endif

            return null;
        }

        /// <summary>
        /// Closes all opened UIs, keeping the last specified number of UIs open.
        /// </summary>
        /// <param name="last">The number of UIs to keep open.</param>
        public void CloseAllUI(int last = 0) {
            if (openedUiList != null) {
                while (openedUiList.Count > last) {
                    openedUiList.Last.Value.CloseUI();

#if UNITY_EDITOR
                    Debug.Log($"Closed UI: {openedUiList.Last.Value.GetType().Name}");
#endif
                }
            }
        }

        /// <summary>
        /// Updates all opened UIs.
        /// </summary>
        public void UpdateUI() {
            if (openedUiList != null) {
                foreach (var ui in openedUiList) {
                    ui.UpdateUI();

#if UNITY_EDITOR
                    Debug.Log($"Updated UI: {ui.GetType().Name}");
#endif
                }
            }
        }
    }

    /// <summary>
    /// Represents canvas options.
    /// </summary>
    [Serializable]
    public struct CanvasOption {
        /// <summary>
        /// The render mode of the canvas.
        /// </summary>
        public RenderMode renderMode;

        /// <summary>
        /// Indicates whether the canvas has a raycaster.
        /// </summary>
        public bool isRaycaster;

        /// <summary>
        /// Initializes a new instance of the CanvasOption struct.
        /// </summary>
        /// <param name="mode">The render mode of the canvas.</param>
        /// <param name="isRaycast">Whether the canvas has a raycaster.</param>
        public CanvasOption(RenderMode mode = RenderMode.ScreenSpaceCamera, bool isRaycast = true) {
            renderMode = mode;
            isRaycaster = isRaycast;
        }
    }
}
