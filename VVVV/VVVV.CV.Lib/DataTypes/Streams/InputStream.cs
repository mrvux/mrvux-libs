using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using VVVV.CV.Lib.DataTypes.Streams;

namespace VVVV.CV.Lib.DataTypes
{
    public class InputStream<T> : BaseInputStream where T : IBaseStream,new()
    {
        protected Pin<T> internalpin;

        protected OutputStream<T> source;

        public bool IsValid
        {
            get 
            {
                if (this.source != null)
                {
                    return this.source.Data.IsValid;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsChanged
        {
            get { return false;} //return this.source.IsChanged; }
        }

        public T Data
        {
            get { return this.source.Data; }
        }


        internal void Connect(OutputStream<T> source)
        {
            this.source = source;
        }

        public override void Connect(object source)
        {
            this.source = (OutputStream<T>)source;
        }

        public override void Disconnect()
        {
            this.source = null;
        }

    }
}
