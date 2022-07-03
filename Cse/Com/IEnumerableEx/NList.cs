using System.Collections.Generic;

namespace CSELibrary.Com.IEnumerableEx
{
    public class NList<T> : List<T>
    {
        /// <summary>
        /// 次のアイテム
        /// </summary>
        public T nextItem{
            get { return GetNext(IndexOf(_nextItem)); }
            set { _nextItem = value; }
        }
        private T _nextItem;

        /// <summary>
        /// 前のアイテム
        /// </summary>
        public T prevItem
        {
            get { return GetNext(IndexOf(_prevItem)); }
            set { _prevItem = value; }
        }
        private T _prevItem;

        public T GetNext(T t) {
            var count = IndexOf(t) + 1;
            if (count > Count) { return default; }
            return this[count];
        }

        public bool IsLast(T t)
        {
            var count = IndexOf(t) + 1;
            if (count == Count) { 
                return true; 
            }
            return false;
        }

        public T GetNext(int nextIndex)
        {
            var count = nextIndex + 1;
            if (count > Count) { return default; }
            return this[count];
        }

        public T GetPrev(T t)
        {
            var count = IndexOf(t) - 1;
            if (count < 0) { return default; }
            return this[count];
        }

        public T GetPrev(int nextIndex)
        {
            var count = nextIndex - 1;
            if (count < 0) { return default; }
            return this[count];
        }
    }
}
