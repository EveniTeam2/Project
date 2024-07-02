#if UNITY_EDITOR
using System.Collections.Generic;
using Core.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestScripts
{
    public class UITestSceneManager : MonoBehaviour
    {
        [SerializeField] private List<AssetReference> m_Assets;

        private void Start() {
            UIManager.Instance.SetData(m_Assets);
            // show up test scene UIs
        }
    }
}
#endif