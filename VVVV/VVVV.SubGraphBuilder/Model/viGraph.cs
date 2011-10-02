using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.PluginInterfaces.V2;
using VVVV.PluginInterfaces.V1;

namespace VVVV.SubGraphBuilder.Model
{
    public class viGraph
    {
        public viGraph()
        {
            this.Nodes = new List<viNode>();

        }

        public List<viNode> Nodes { get; set; }

        public viNode FindNode(INode2 hdenode)
        {
            foreach (viNode n in this.Nodes)
            {
                if (n.HdeNode == hdenode.InternalCOMInterf)
                {
                    return n;
                }
            }
            return null;
        }

        public viPin FindPin(IPluginIO instance)
        {
            foreach (viNode n in this.Nodes)
            {
                foreach (viInputPin i in n.InputPins)
                {
                    if (i.ComInstance == instance)
                    {
                        return i;
                    }
                }
                foreach (viOutputPin i in n.OutputPins)
                {
                    if (i.ComInstance == instance)
                    {
                        return i;
                    }
                }
            }
            return null;
        }

        public viPin FindPin(IPin hdepin)
        {
            foreach (viNode n in this.Nodes)
            {
                foreach (viInputPin i in n.InputPins)
                {
                    if (i.HdePin == hdepin)
                    {
                        return i;
                    }
                }
                foreach (viOutputPin i in n.OutputPins)
                {
                    if (i.HdePin == hdepin)
                    {
                        return i;
                    }
                }
            }
            return null;
        }

    }
}
