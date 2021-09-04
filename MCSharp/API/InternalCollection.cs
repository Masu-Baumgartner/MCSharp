using System;
using System.Collections.Generic;

namespace MCSharp.API
{
    public class InternalCollection<type>
    {
        internal type[] objs = new type[1];
        public type this[int i]
        {
            get
            {
                return objs[i];
            }
            internal set
            {
                objs[i] = value;
            }
        }
        internal void SetSize(int newSize)
        {
            type[] newo = new type[newSize];
            int i = 0;
            foreach(var v in objs)
            {
                try
                {
                    newo[i] = objs[i++];
                }
                catch (Exception)
                {

                }
            }
            objs = newo;
        }
        public void ForEach(Action<type> action)
        {
            foreach (var v in this)
            {
                action(v);
            }
        }
        public IEnumerator<type> GetEnumerator()
        {
            return (IEnumerator<type>)objs.GetEnumerator();
        }
    }
}