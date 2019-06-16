using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BackEndAssignment.Helpers
{
    /// <summary>
    /// A queue which dequeues higher priority elements first
    /// </summary>
    /// <typeparam name="TElement">Type of element to store</typeparam>
    /// <typeparam name="TPriority">Type of priority of the element. Must be IComparable (e.g. int)</typeparam>
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private SortedDictionary<TPriority, Queue<TElement>> dictionary;
        private static object _syncObj = new object();

        public PriorityQueue()
        {
            dictionary = new SortedDictionary<TPriority, Queue<TElement>>(new DescendingComparer<TPriority>());
        }

        /// <summary>
        /// Adds an element to the queue.
        /// </summary>
        /// <param name="item">Item to be added.</param>
        /// <param name="priority">Priority of the item. Higher priority will dequeue first.</param>
        public void Enqueue(TElement item, TPriority priority)
        {
            TPriority key = priority;
            Queue<TElement> queue;
            
            //synchronized to avoid empty queues in the dictionary.
            lock (_syncObj)
            {
                if (!dictionary.TryGetValue(key, out queue))
                {
                    queue = new Queue<TElement>(300);
                    dictionary.Add(key, queue);
                }

                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Removes an element with high priority from the queue.
        /// </summary>
        /// <returns>High priority item.</returns>
        public TElement Dequeue()
        {
            if (Count == 0)
                throw new Exception("No items to Dequeue:");
            KeyValuePair<TPriority, Queue<TElement>> first = dictionary.First();
            TPriority key = first.Key;

            Queue<TElement> queue = first.Value;
            TElement output;
            
            //synchronized to avoid empty queues in the dictionary.
            lock (_syncObj)
            {
                output = queue.Dequeue();
                if (queue.Count == 0)
                    dictionary.Remove(key);
            }
            return output;
        }

        /// <summary>
        /// Number of elements in the queue
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                foreach (Queue<TElement> queue in dictionary.Values)
                {
                    count += queue.Count;
                }
                return count;
            }
        }
    }


    class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }
}