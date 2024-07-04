using UnityEngine;

namespace Core.Utils {
    /// <summary>
    /// Call anytime when you want.
    /// </summary>
    /// <typeparam name="T">type that you want to make singleton</typeparam>
    public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T> {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null) {
                    var obj = GameObject.Instantiate(new GameObject()).AddComponent<T>();
                    DontDestroyOnLoad(obj);
                    obj.Initialize();
                    _instance = obj;
                }
                return _instance;
            }
        }
        protected virtual void Awake() {
            if (_instance != null) {
                if (!ReferenceEquals(_instance, this))
                    Destroy(this);
                return;
            }
            else {
                _instance = (T)this;
                DontDestroyOnLoad(_instance);
                Initialize();
            }
        }
        protected abstract T Initialize();
    }
}
