using System;
using System.Diagnostics;

namespace AutoPatcher.Util
{
    internal sealed class LockedReference<T>
    {
        private readonly object syncObj = new object();
        private T obj;

        public LockedReference(T obj = default(T))
        {
            this.obj = obj;
        }

        // Do not allow reference to the object to escape!!
        public void AccessReference(Func<T, T> action)
        {
            Debug.Assert(action != null);

            lock (this.syncObj)
            {
                this.obj = action(obj);
            }
        }
    }
}
