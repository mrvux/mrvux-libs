using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrVux.Utilities.Reflection;
using NTP.Utilities.Reflection;
using System.Reflection;
using VVVV.CV.Lib.Interfaces;

namespace VVVV.Imaging.Poc.Factory
{
    public class StreamProcessorLocator : BaseTypeLocator<IStreamProcessor>
    {
        public StreamProcessorLocator()
            : base(new TypeLookup())
        {

        }

        public Dictionary<string, Type> RegisteredTypes = new Dictionary<string, Type>();

        protected override bool RegisterType(Type type)
        {
            if (AttributesHelper.HasSingleAttribute<StreamProcessorInfoAttribute>(type))
            {
                StreamProcessorInfoAttribute attr = AttributesHelper.GetSingleAttribute<StreamProcessorInfoAttribute>(type);
                this.RegisteredTypes.Add(attr.Name, type);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Scan(Assembly a)
        {
            this.RegisteredTypes.Clear();
            this.types.Clear();
            this.Lookup(a);
        }
    }
}
