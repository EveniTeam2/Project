using System.Collections.Generic;
using UnityEngine;

namespace Core.Utils
{
    [System.Serializable]
    public class SerializableQueue<T>
    {
        [SerializeField]
        private List<T> list = new();
        private Queue<T> _queue;

        public Queue<T> Queue
        {
            get
            {
                if (_queue == null)
                {
                    _queue = new Queue<T>(list);
                }
                return _queue;
            }
        }

        public void Enqueue(T item)
        {
            Queue.Enqueue(item);
            list.Add(item);
        }

        public T Dequeue()
        {
            var item = Queue.Dequeue();
            list.Remove(item);
            return item;
        }

        public void Clear()
        {
            while (Queue.Count > 0)
            {
                var item = Queue.Dequeue();
                list.Remove(item);
            }
        }

        public int Count => Queue.Count;
    }
}