namespace Core.Utils {
    /// <summary>
    /// Call anytime when you want
    /// </summary>
    /// <typeparam name="T">type that you want to make singleton</typeparam>
    public abstract class PlainSingleton<T> where T : PlainSingleton<T>, new() {
        protected static T instance;
        public static T Instance {
            get {
                if (instance == null) {
                    var obj = new T();
                    obj.Initialize();
                    instance = obj;
                }
                return instance;
            }
        }
        protected abstract void Initialize();
    }
}
