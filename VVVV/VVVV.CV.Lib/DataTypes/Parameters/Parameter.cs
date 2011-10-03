using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V1;
using VVVV.Hosting.Pins;
using VVVV.CV.Lib.DataTypes;

namespace VVVV.CV.Lib.DataTypes
{
    public class Parameter<T> : IParameter
    {
        private IPluginHost host;

        private IDiffSpread<T> internaldata;

        private List<T> cache;

        private object m_lock = new object();

        private bool haschanged = true;

        internal Parameter(IPluginHost host, string name)
        {
            this.host = host;

            InputAttribute attr = new InputAttribute(name);

            this.internaldata = PinFactory.CreateDiffSpread<T>(host, attr);

            this.CacheData();
        }

        public bool IsChanged
        {
            get { return this.haschanged; }
        }

       
        public List<T> Data
        {
            get 
            {
                this.haschanged = false;
                return this.cache; 
            }
        }

        /// <summary>
        /// Set Data to cache, make sure we lock so
        /// we have ISpread sync
        /// </summary>
        public void CacheData()
        {
            if (this.internaldata.IsChanged)
            {
                lock (this.m_lock)
                {
                    this.cache = new List<T>(this.internaldata);
                }
                this.haschanged = true;
            }
        }
    }
}
