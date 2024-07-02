using System;
using System.Collections.Generic;
using Core.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.UI
{
    /// <summary>
    /// UIManager 클래스: 모든 UI 작업을 관리합니다.
    /// </summary>
    public class UIManager : UnitySingleton<UIManager>
    {
        private List<AssetReference> uiAssetRef;
        private Dictionary<string, int> typeToAssetRefIndex;
        private LinkedList<UIBase> openedUiList;
        private Dictionary<string, UIBase> uiDict;
        private Dictionary<CanvasOption, CanvasController> canvas;

        protected override UIManager Initialize()
        {
            openedUiList = new LinkedList<UIBase>();
            uiDict = new Dictionary<string, UIBase>();
            typeToAssetRefIndex = new Dictionary<string, int>();
            return this;
        }

        public UIManager SetData(List<AssetReference> uiAssetRef)
        {
            this.uiAssetRef = uiAssetRef;
            return this;
        }

        private void Start()
        {
            LoadUI<GameObject>();
        }

        void LoadUI<T>(Action<T> onComplete = null)
        {
            for (int i = 0; i < uiAssetRef.Count; ++i)
            {
                int index = i;
                AddressableLoader.LoadAsset<GameObject>(uiAssetRef[i], obj =>
                {
                    if (obj.TryGetComponent<T>(out T ui))
                    {
                        typeToAssetRefIndex.Add(ui.GetType().Name, index);
                        onComplete?.Invoke(ui);
                    }
                });
            }
        }

        void DeployUI<T>(AssetReference assetRef, Action<T> onComplete = null) where T : UIBase
        {
            AddressableLoader.DeployAsset(assetRef, Vector3.zero, Quaternion.identity, null, obj =>
            {
                if (obj.TryGetComponent<T>(out T ui))
                {
                    if (openedUiList == null)
                    {
                        openedUiList = new LinkedList<UIBase>();
                    }
                    if (uiDict == null)
                    {
                        uiDict = new Dictionary<string, UIBase>();
                    }

                    ui.ActOnDraw += () => openedUiList.AddLast(ui);
                    ui.ActOnClose += () => openedUiList.Remove(ui);
                    uiDict.Add(ui.GetType().Name, ui);
                    ui.InitUI();
                    if (obj.TryGetComponent(out RectTransform uiRectTransform))
                    {
                        if (canvas == null)
                        {
                            canvas = new Dictionary<CanvasOption, CanvasController>();
                        }
                        uiRectTransform.SetParent(canvas[ui.GetCanvasOption()].transform);
                        uiRectTransform.localEulerAngles = Vector3.zero;
                        uiRectTransform.localPosition = Vector3.zero;
                        uiRectTransform.localScale = Vector3.one;
                        uiRectTransform.sizeDelta = Vector2.zero;
                    }
                    onComplete?.Invoke(ui);
                }
            });
        }

        public T TryGetUI<T>(string typeName, Action<T> onLoadComplete = null) where T : UIBase
        {
            if (uiDict != null && uiDict.TryGetValue(typeName, out var ui))
            {
                onLoadComplete?.Invoke(ui as T);
                return ui as T;
            }

            if (typeToAssetRefIndex != null && typeToAssetRefIndex.TryGetValue(typeName, out int index))
            {
                DeployUI<T>(uiAssetRef[index], onLoadComplete);
            }
            else
            {
                LoadUI<T>(ui => DeployUI(uiAssetRef[typeToAssetRefIndex[typeName]], onLoadComplete));
            }

            return null;
        }

        public void RegisterCanvas(CanvasController cv, CanvasOption opt)
        {
            if (canvas == null)
            {
                canvas = new Dictionary<CanvasOption, CanvasController>();
            }

            if (!canvas.TryAdd(opt, cv))
            {
                GameObject.Destroy(cv);
            }
        }

        public UIBase TryGetTopUI()
        {
            if (openedUiList != null && openedUiList.Count > 0)
            {
                return openedUiList.Last.Value;
            }
            return null;
        }

        public void UpdateSize(float fontScale, float sizeScale)
        {
            if (openedUiList != null)
            {
                foreach (UIBase ui in openedUiList)
                {
                    ui.SetFontScale(fontScale);
                    ui.SetScale(sizeScale);
                }
            }
        }

        public Canvas TryGetCanvas(CanvasOption opt)
        {
            if (canvas != null && canvas.TryGetValue(opt, out var ui))
            {
                return ui.GetCanvasComponent();
            }
            return null;
        }

        public void CloseAllUI(int last = 0)
        {
            if (openedUiList != null)
            {
                while (openedUiList.Count > last)
                {
                    openedUiList.Last.Value.CloseUI();
                }
            }
        }

        public void UpdateUI()
        {
            if (openedUiList != null)
            {
                foreach (var ui in openedUiList)
                {
                    ui.UpdateUI();
                }
            }
        }
    }

    /// <summary>
    /// CanvasOption 구조체: 캔버스 옵션을 나타냅니다.
    /// </summary>
    [Serializable]
    public struct CanvasOption
    {
        public RenderMode renderMode;
        public bool isRaycaster;

        public CanvasOption(RenderMode mode = RenderMode.ScreenSpaceCamera, bool isRaycast = true)
        {
            renderMode = mode;
            isRaycaster = isRaycast;
        }
    }
}