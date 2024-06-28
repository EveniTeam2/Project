using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using UnityEditor;

public class UITestSceneManager : MonoBehaviour
{
    [SerializeField] private List<AssetReference> m_Assets;

    private void Start() {
        UI.UIManager.Instance.SetData(m_Assets);
        // show up test scene UIs
    }
}
#endif