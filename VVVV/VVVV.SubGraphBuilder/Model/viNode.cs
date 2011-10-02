using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.Hosting.Pins.Output;
using VVVV.PluginInterfaces.V2;
using VVVV.Hosting;

namespace VVVV.SubGraphBuilder.Model
{
    /// <summary>
    /// Wraps vvvv node info and instance
    /// </summary>
    public class viNode
    {
        public viNode()
        {
            this.InputPins = new List<viInputPin>();
            this.OutputPins = new List<viOutputPin>();
        }

        public IPluginHost Hoster { get; set; }
        public INode HdeNode { get; set; }

        public List<viInputPin> InputPins { get; set; }
        public List<viOutputPin> OutputPins { get; set; }

        #region Instance Node
        public T Instance<T>()
        {
            IInternalPluginHost iip = (IInternalPluginHost)this.Hoster;
            //ipp.
            return (T)iip.Plugin;
        }

        public bool IsAssignable<T>()
        {
            IInternalPluginHost iip = (IInternalPluginHost)this.Hoster;
            //ipp.
            return typeof(T).IsAssignableFrom(iip.Plugin.GetType());
        }
        #endregion

        #region Removes a Pin
        public bool RemovePin(string name)
        {
            viInputPin ip = null;
            foreach (viInputPin vi in this.InputPins)
            {
                if (vi.Name == name)
                {
                    ip = vi;
                }
            }
            if (ip != null)
            {
                //Diconnect parent if applicable
                if (ip.ParentPin != null)
                {
                    if (ip.ParentPin.ChildrenPins.Contains(ip))
                    {
                        ip.ParentPin.ChildrenPins.Remove(ip);
                    }
                }

                this.InputPins.Remove(ip);
            }

            viOutputPin op = null;
            foreach (viOutputPin vo in this.OutputPins)
            {
                if (vo.Name == name)
                {
                    op = vo;
                }
            }
            if (op != null)
            {
                foreach (viInputPin vip in op.ChildrenPins)
                {
                    vip.Disconnect(op);
                }

                this.OutputPins.Remove(op);
            }

            return ip != null || op != null;
        }
        #endregion
    }
}
