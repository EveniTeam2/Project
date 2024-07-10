using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils
{
    public class CustomPool<T> where T : MonoBehaviour
    {
        private T obj;
        private Transform root;
        private Action<T, CustomPool<T>> create;
        private Action<T> get;
        private Action<T> release;
        private Action<T> destroy;
        private Queue<T> pool;
        private LinkedList<T> usedObjs;
        private int size;
        private bool flexible;

        public int Size => size;
        public int UsedCount => usedObjs.Count;
        public int UnusedCount => pool.Count;
        public LinkedList<T> UsedList => usedObjs;

        public CustomPool(T prefab, Transform root, Action<T, CustomPool<T>> createAction, Action<T> getAction,
            Action<T> releaseAction, Action<T> destoryAction, int size, bool isFlexible)
        {
            obj = prefab;
            this.root = root;
            create = createAction;
            get = getAction;
            release = releaseAction;
            destroy = destoryAction;
            pool = new Queue<T>();
            this.size = size;
            flexible = isFlexible;
            usedObjs = new LinkedList<T>();

            for (var i = 0; i < size; ++i)
            {
                Create();
            }
        }

        public static CustomPool<T> MakePool(T prefab, Transform root = null, Action<T, CustomPool<T>> create = null, Action<T> get = null,
            Action<T> release = null, Action<T> destroy = null, int poolSize = 10, bool isFlexible = true)
        {
            return new CustomPool<T>(prefab, root, create, get, release, destroy, poolSize, isFlexible);
        }

        private T Create()
        {
            var item = GameObject.Instantiate(obj, root);
            pool.Enqueue(item);
            create?.Invoke(item, this);
            return item;
        }

        public T Get()
        {
            T item;
            if (pool.Count <= 0)
            {
                if (flexible)
                {
                    item = GameObject.Instantiate(obj, root);
                    create?.Invoke(item, this);
                }
                else
                {
                    item = usedObjs.First.Value;
                    usedObjs.RemoveFirst();
                    release?.Invoke(item);
                }
            }
            else
                item = pool.Dequeue();

            get?.Invoke(item);
            usedObjs.AddLast(item);
            return item;
        }

        public void ReleaseAll()
        {
            while (usedObjs.Count > 0)
                Release(usedObjs.First.Value);
        }

        public void Release(T item)
        {
            if (usedObjs.Remove(item))
            {
                release?.Invoke(item);
                pool.Enqueue(item);
            }
        }

        public void DestroyPool()
        {
            ReleaseAll();
            foreach (var item in pool)
            {
                Destroy(item);
            }
            pool.Clear();
        }

        private void Destroy(T item)
        {
            destroy?.Invoke(item);
            GameObject.Destroy(item.gameObject);
        }
    }
}