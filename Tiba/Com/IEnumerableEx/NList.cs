using System.Collections.Generic;

namespace NEETLibrary.Tiba.Com.IEnumerableEx
{
    public class NList<T> : List<T>
    {
        public T GetNext(T t) {
            var count = this.IndexOf(t) + 1;
            if (count > this.Count) { return default; }
            return this[count];
        }

        public T GetNext(int nextIndex)
        {
            var count = nextIndex + 1;
            if (count > this.Count) { return default; }
            return this[count];
        }

        public T GetPrev(T t)
        {
            var count = this.IndexOf(t) - 1;
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
