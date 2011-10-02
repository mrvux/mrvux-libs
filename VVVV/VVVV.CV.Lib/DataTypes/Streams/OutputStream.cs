using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.CV.Lib.DataTypes
{
    public class OutputStream<T> : InternalStream<T> where T : IBaseStream, new()
    {
        protected T data = new T();
        private bool changed;

        public OutputStream()
        {
            this.changed = false;
        }

        internal void Reset()
        {
            this.changed = false;
        }

        public void Invalidate()
        {
            this.changed = true;
        }

        public bool IsChanged
        {
            get { return this.changed; }
        }

        public T Data
        {
            get { return data; }
        }
    }
}
