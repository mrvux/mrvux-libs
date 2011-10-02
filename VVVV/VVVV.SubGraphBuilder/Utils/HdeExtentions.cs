using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.PluginInterfaces.V2;

namespace VVVV
{
    public static class HdeExtentions
    {
        #region Is GetSlice (Node)
        public static bool IsGetSliceNode(this INode2 hdenode)
        {
            if (hdenode.NodeInfo.Type == NodeType.Native && hdenode.NodeInfo.Name == "GetSlice" && hdenode.NodeInfo.Category == "Node")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region INode Assignable from
        public static bool IsNodeAssignableFrom<T>(this INode2 node)
        {
            if (node.NodeInfo.Type == NodeType.Plugin || node.NodeInfo.Type == NodeType.Dynamic)
            {
                try
                {
                    string path = System.IO.Path.GetFileNameWithoutExtension(node.NodeInfo.Filename);
                    Type t = Type.GetType(node.NodeInfo.Arguments + "," + path);
                    if (t == null) { t = (Type)node.NodeInfo.UserData; }

                    return typeof(T).IsAssignableFrom(t);
                }
                catch
                {
                    return false;

                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Find Hde Pin
        public static IPin FindHdePinByName(this INode node, string name)
        {
            foreach (IPin pin in node.GetPins())
            {
                if (pin.GetName() == name)
                {
                    return pin;
                }
            }
            return null;
        }
        #endregion

    }
}
