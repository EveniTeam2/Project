using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
#if UNITY_2018_1_OR_NEWER
#endif

#if UNITY_2018_1_OR_NEWER
namespace GoogleMobileAds.Editor
{
    public class BuildPreProcessor : IPreprocessBuildWithReport
#else
public class BuildPreProcessor : IPreprocessBuild
#endif
    {

        public int callbackOrder { get { return 1; } }

#if UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(BuildReport report)
#else
    public void OnPreprocessBuild(BuildTarget target, string path)
#endif
        {
            if (!AssetDatabase.IsValidFolder("Assets/GoogleMobileAds"))
            {
                AssetDatabase.CreateFolder("Assets", "GoogleMobileAds");
            }

            /*
         * Handle importing GMA via Unity Package Manager.
         */
            EditorPathUtils pathUtils = ScriptableObject.CreateInstance<EditorPathUtils>();
            if (pathUtils.IsPackageRootPath())
            {
                string parentDirectoryPath = pathUtils.GetParentDirectoryAssetPath();
                string linkXmlPath = Path.Combine(parentDirectoryPath, "link.xml");

                /*
             * Copy link.xml to Assets/GoogleMobileAds to ensure all platform dependent libraries
             * are included in the build.
             */
                AssetDatabase.CopyAsset(linkXmlPath, "Assets/GoogleMobileAds/link.xml");
            }
        }
    }
}
