using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;

namespace VVVV.SubGraphBuilder.Model
{
    public class viInternalLink
    {
        public IPluginHost Host;
        public IPluginIO Instance;
        public IPin Otherpin;
    }
}
