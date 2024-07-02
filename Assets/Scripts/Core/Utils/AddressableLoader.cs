using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Utils {
    /// <summary>
    /// 애드레서블 자산 시스템을 관리하기 위한 유틸리티 클래스입니다.
    /// </summary>
    public class AddressableLoader {
        private static Dictionary<GameObject, AsyncOperationHandle<GameObject>> instantiatedObjects = new Dictionary<GameObject, AsyncOperationHandle<GameObject>>();

        /// <summary>
        /// AssetReference를 사용하여 T 유형의 자산을 로드합니다.
        /// </summary>
        /// <typeparam name="T">로드할 자산의 유형입니다.</typeparam>
        /// <param name="assetRef">로드할 자산의 AssetReference입니다.</param>
        /// <param name="onComplete">자산이 로드되었을 때 호출할 작업입니다.</param>
        public static void LoadAsset<T>(AssetReference assetRef, Action<T> onComplete = null) {
            if (assetRef.RuntimeKeyIsValid()) {
                if (assetRef.Asset is T ret) {
                    onComplete?.Invoke(ret);
                }
                else {
                    var asyncOpHandle = Addressables.LoadAssetAsync<T>(assetRef);
#if UNITY_EDITOR
                    Debug.Log($"[{nameof(AddressableLoader)}] 주소를 사용하여 자산을 로드 중: {assetRef.RuntimeKey}");
#endif
                    asyncOpHandle.Completed += (op) => {
                        if (op.Status == AsyncOperationStatus.Succeeded) {
                            onComplete?.Invoke(op.Result);
                        }
                        else {
#if UNITY_EDITOR
                            Debug.LogError($"주소를 사용하여 자산 로드에 실패했습니다: {assetRef.RuntimeKey}");
#endif
                        }
                    };
                }
            }
            else {
#if UNITY_EDITOR
                Debug.LogWarning($"유효하지 않은 AssetReference: {assetRef}");
#endif
            }
        }

        /// <summary>
        /// AssetReference를 사용하여 GameObject를 인스턴스화합니다.
        /// </summary>
        /// <param name="assetRef">인스턴스화할 GameObject의 AssetReference입니다.</param>
        /// <param name="position">GameObject를 인스턴스화할 위치입니다.</param>
        /// <param name="quaternion">GameObject를 인스턴스화할 회전입니다.</param>
        /// <param name="onComplete">GameObject가 인스턴스화되었을 때 호출할 작업입니다.</param>
        /// <returns>인스턴스화가 성공하면 true, 그렇지 않으면 false를 반환합니다.</returns>
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
                    Debug.Log($"[{nameof(AddressableLoader)}] 주소를 사용하여 자산을 인스턴스화 중: {assetRef.RuntimeKey}");
#endif
                    asyncOpHandle.Completed += (op) => {
                        if (op.Status == AsyncOperationStatus.Succeeded) {
                            instantiatedObjects[op.Result] = asyncOpHandle;
                            onComplete?.Invoke(op.Result);
                        }
                        else {
#if UNITY_EDITOR
                            Debug.LogError($"주소를 사용하여 자산 인스턴스화에 실패했습니다: {assetRef.RuntimeKey}");
#endif
                        }
                    };
                    return true;
                }
            }
            else {
#if UNITY_EDITOR
                Debug.LogWarning($"유효하지 않은 AssetReference: {assetRef}");
#endif
                return false;
            }
        }

        /// <summary>
        /// GameObject가 AssetReference를 통해 인스턴스화되었는지 확인합니다.
        /// </summary>
        /// <param name="obj">확인할 GameObject입니다.</param>
        /// <returns>GameObject가 AssetReference를 통해 인스턴스화되었으면 true, 그렇지 않으면 false를 반환합니다.</returns>
        public static bool IsInstantiatedByAddressable(GameObject obj) {
            return instantiatedObjects.ContainsKey(obj);
        }

        /// <summary>
        /// 인스턴스화된 GameObject와 관련된 AssetReference를 해제합니다.
        /// </summary>
        /// <param name="instantiatedObject">해제할 GameObject입니다.</param>
        public static void ReleaseAsset(GameObject instantiatedObject) {
            if (instantiatedObjects.TryGetValue(instantiatedObject, out var handle)) {
                Addressables.ReleaseInstance(instantiatedObject);
                Addressables.Release(handle);
                instantiatedObjects.Remove(instantiatedObject);
#if UNITY_EDITOR
                Debug.Log($"자산 및 인스턴스 해제: {instantiatedObject.name}");
#endif
            }
            else {
#if UNITY_EDITOR
                Debug.LogWarning($"AddressableLoader를 통해 인스턴스화되지 않은 개체를 해제하려고 시도했습니다: {instantiatedObject.name}");
#endif
            }
        }
    }
}