using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace Utils {
    /// <summary>
    /// Utils for managing Addressable Asset System.
    /// </summary>
    public class AddressableLoader {
        private static Dictionary<GameObject, AsyncOperationHandle<GameObject>> instantiatedObjects = new Dictionary<GameObject, AsyncOperationHandle<GameObject>>();

        /// <summary>
        /// Loads an asset of type T using an AssetReference.
        /// </summary>
        /// <typeparam name="T">The type of the asset to load.</typeparam>
        /// <param name="assetRef">The AssetReference of the asset to load.</param>
        /// <param name="onComplete">Action to invoke when the asset is loaded.</param>
        public static void LoadAsset<T>(AssetReference assetRef, Action<T> onComplete = null) {
            if (assetRef.RuntimeKeyIsValid()) {
                if (assetRef.Asset is T ret) {
                    onComplete?.Invoke(ret);
                }
                else {
                    var asyncOpHandle = Addressables.LoadAssetAsync<T>(assetRef);
#if UNITY_EDITOR
                    Debug.Log($"[{nameof(AddressableLoader)}] Loading asset with address: {assetRef.RuntimeKey}");
#endif
                    asyncOpHandle.Completed += (op) => {
                        if (op.Status == AsyncOperationStatus.Succeeded) {
                            onComplete?.Invoke(op.Result);
                        }
                        else {
#if UNITY_EDITOR
                            Debug.LogError($"Failed to load asset with address: {assetRef.RuntimeKey}");
#endif
                        }
                    };
                }
            }
            else {
#if UNITY_EDITOR
                Debug.LogWarning($"Invalid AssetReference: {assetRef}");
#endif
            }
        }


        /// <summary>
        /// Instantiates a GameObject using an AssetReference.
        /// </summary>
        /// <param name="assetRef">The AssetReference of the GameObject to instantiate.</param>
        /// <param name="position">The position to instantiate the GameObject at.</param>
        /// <param name="quaternion">The rotation to instantiate the GameObject with.</param>
        /// <param name="onComplete">Action to invoke when the GameObject is instantiated.</param>
        /// <returns>True if the instantiation is successful, false otherwise.</returns>
        public static bool DeployAsset(AssetReference assetRef, Vector3 position, Quaternion quaternion, Transform parent = null, Action<GameObject> onComplete = null) {
            if (assetRef.RuntimeKeyIsValid()) {
                if (assetRef.Asset is GameObject ret) {
                    GameObject instantiatedObject;
                    if (parent != null)
                        instantiatedObject = GameObject.Instantiate(ret, position, quaternion, parent);
                    else
                        instantiatedObject = GameObject.Instantiate(ret, position, quaternion);
                    onComplete?.Invoke(instantiatedObject);
                    return true;
                }
                else {
                    var asyncOpHandle = parent != null ? Addressables.InstantiateAsync(assetRef, position, quaternion, parent) : Addressables.InstantiateAsync(assetRef, position, quaternion);
#if UNITY_EDITOR
                    Debug.Log($"[{nameof(AddressableLoader)}] Instantiating asset with address: {assetRef.RuntimeKey}");
#endif
                    asyncOpHandle.Completed += (op) => {
                        if (op.Status == AsyncOperationStatus.Succeeded) {
                            instantiatedObjects[op.Result] = asyncOpHandle;
                            onComplete?.Invoke(op.Result);
                        }
                        else {
#if UNITY_EDITOR
                            Debug.LogError($"Failed to instantiate asset with address: {assetRef.RuntimeKey}");
#endif
                        }
                    };
                    return true;
                }
            }
            else {
#if UNITY_EDITOR
                Debug.LogWarning($"Invalid AssetReference: {assetRef}");
#endif
                return false;
            }
        }

        /// <summary>
        /// Checks if a GameObject was instantiated via an AssetReference.
        /// </summary>
        /// <param name="obj">The GameObject to check.</param>
        /// <returns>True if the GameObject was instantiated via an AssetReference, false otherwise.</returns>
        public static bool IsInstantiatedByAddressable(GameObject obj) {
            return instantiatedObjects.ContainsKey(obj);
        }

        /// <summary>
        /// Releases an instantiated GameObject and its associated AssetReference.
        /// </summary>
        /// <param name="instantiatedObject">The GameObject to release.</param>
        public static void ReleaseAsset(GameObject instantiatedObject) {
            if (instantiatedObjects.TryGetValue(instantiatedObject, out var handle)) {
                Addressables.ReleaseInstance(instantiatedObject);
                Addressables.Release(handle);
                instantiatedObjects.Remove(instantiatedObject);
#if UNITY_EDITOR
                Debug.Log($"Released asset and instance for: {instantiatedObject.name}");
#endif
            }
            else {
#if UNITY_EDITOR
                Debug.LogWarning($"Attempted to release an object that was not instantiated by AddressableLoader: {instantiatedObject.name}");
#endif
            }
        }
    }
}