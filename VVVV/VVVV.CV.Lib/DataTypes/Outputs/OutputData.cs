using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V1;
using VVVV.Hosting.Pins;

namespace VVVV.CV.Lib.DataTypes
{
    public class OutputData<T> : IOutputData
    {
        private IPluginHost host;

        private ISpread<T> internaldata;

        private List<T> cache;

        private object m_lock = new object();

        internal OutputData(IPluginHost host, string name)
        {
            this.host = host;

            OutputAttribute attr = new OutputAttribute(name);

            this.internaldata = PinFactory.CreateSpread<T>(host, attr);

            this.cache = new List<T>();
        }

        public List<T> Data
        {
            get { return this.cache; }
        }

        /// <summary>
        /// Set Data to cache, make sure we lock so
        /// we have ISpread sync
        /// </summary>
        public void CopyOutput()
        {
            lock (this.m_lock)
            {
                this.internaldata.AssignFrom(this.cache);
            }
        }
    }
}
