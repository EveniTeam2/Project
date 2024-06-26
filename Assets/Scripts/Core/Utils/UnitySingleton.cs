using UnityEngine;

namespace Utils {
    /// <summary>
    /// Call anytime when you want.
    /// </summary>
    /// <typeparam name="T">type that you want to make singleton</typeparam>
    public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T> {
        private static T instance;
        public static T Instance {
            get {
                if (instance == null) {
                    var obj = GameObject.Instantiate(new GameObject()).AddComponent<T>();
                    DontDestroyOnLoad(obj);
                    obj.Initialize();
                    instance = obj;
                }
                return instance;
            }
        }
        protected virtual void Awake() {
            if (instance != null) {
                if (!ReferenceEquals(instance, this))
                    Destroy(this);
                return;
            }
            else {
                instance = (T)this;
                DontDestroyOnLoad(instance);
                Initialize();
            }
        }
        protected abstract T Initialize();
    }
}
