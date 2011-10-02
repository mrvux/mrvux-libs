using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.PluginInterfaces.V2;

namespace VVVV.SubGraphBuilder.Model
{
    public class viPin
    {
        public viPin(viNode parentnode)
        {
            this.ParentNode = parentnode;
        }

        public string Name { get; set; }
        public viNode ParentNode { get; private set; }
        public IPin HdePin { get; set; }

        //VVVV Info
        public IPluginIO ComInstance { get; set; }

    }
}
