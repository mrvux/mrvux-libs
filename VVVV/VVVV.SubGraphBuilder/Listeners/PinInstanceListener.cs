using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Hosting.Pins.Output;
using VVVV.Hosting.Pins.Input;
using VVVV.Hosting.Pins;
using System.Diagnostics;
using System.Reflection;
using VVVV.SubGraphBuilder.Model;

namespace VVVV.SubGraphBuilder.Listeners
{
    public class PinDictionary : Dictionary<string, IPluginIO> { }
   
    public class PluginPins
    {
        public PluginPins(IPluginHost host)
        {
            this.Host = host;
            this.Inputs = new PinDictionary();
            this.Outputs = new PinDictionary();
        }

        public IPluginHost Host { get; protected set; }

        public PinDictionary Inputs { get; protected set; }

        public PinDictionary Outputs { get; protected set; }

    }
    

    public class PinInstanceListener
    {
        public delegate void PinInstanceEvent(object instance, IPluginHost host, IPluginIO cominstance, TPinDirection direction);

        public delegate void PinConnectionEvent(viInternalLink link);

        public event PinInstanceEvent PinAdded;
        public event PinConnectionEvent InputPinConnect;
        public event PinConnectionEvent InputPinDisconnect;

        public Dictionary<IPluginHost,PluginPins> cache = new Dictionary<IPluginHost,PluginPins>();

        private Dictionary<IPluginIO, IPluginHost> pincache = new Dictionary<IPluginIO, IPluginHost>();

        public PinInstanceListener()
        {

        }

        public void RegisterType<T>()
        {
            PinFactory.RegisterCustomInputPinType(typeof(T), (host, attribute, t) => this.CreateInputPin<T>(host, attribute));
            PinFactory.RegisterCustomOutputPinType(typeof(T), (host, attribute, t) => this.CreateOutputPin<T>(host, attribute));
        }

        public IPluginIO GetInput(IPluginHost host, string name)
        {
            if (this.cache.ContainsKey(host))
            {
                PluginPins pp = this.cache[host];

                if (pp.Inputs.ContainsKey(name))
                {
                    return pp.Inputs[name];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Debug.WriteLine("Can't find host");
                return null;
            }
        }

        public IPluginIO GetOutput(IPluginHost host, string name)
        {
            if (this.cache.ContainsKey(host))
            {
                PluginPins pp = this.cache[host];

                if (pp.Outputs.ContainsKey(name))
                {
                    return pp.Outputs[name];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Debug.WriteLine("Can't find host");
                return null;
            }
        }


        //Use from here to clean cache
        public void RemovePin(IPluginHost host, string name)
        {
            if (this.cache.ContainsKey(host))
            {
                PluginPins pp = this.cache[host];

                if (pp.Inputs.ContainsKey(name))
                {
                    pp.Inputs.Remove(name);
                }
                else
                {
                    if (pp.Outputs.ContainsKey(name))
                    {
                        pp.Outputs.Remove(name);
                    }
                }
            }
        }

        //Clean cache for removed node
        public void RemoveNode(IPluginHost host)
        {
            if (this.cache.ContainsKey(host)) { this.cache.Remove(host); }
        }

        #region Create Input Pin
        private object CreateInputPin<T>(IPluginHost host, InputAttribute attr)// where T : DX11ResourcePin
        {
            //Create pin, and intercept connect/disconnect event
            GenericInputPin<T> pin = new GenericInputPin<T>(host, attr);
            pin.Connected += this.InputPinConnected;
            pin.Disconnected += this.InputPinDisconnected;

            //Add to cache
            PluginPins pp;
            if (!this.cache.ContainsKey(host))
            {
                pp = new PluginPins(host);
                this.cache.Add(host, pp);
            }
            else
            {
                pp = this.cache[host];
            }

            pp.Inputs[attr.Name] = pin.PluginIO;

            this.pincache.Add(pin.PluginIO, host);
            
            if (this.PinAdded != null) { this.PinAdded(pin, host, pin.PluginIO,TPinDirection.Input); }
            return pin;
        }
        #endregion

        #region Create output pin
        private object CreateOutputPin<T>(IPluginHost host, OutputAttribute attr)// where T : IDX11Resource
        {

            GenericOutputPin<T> pin = new GenericOutputPin<T>(host, attr);

            PluginPins pp;
            if (!this.cache.ContainsKey(host))
            {
                pp = new PluginPins(host);
                this.cache.Add(host, pp);
            }
            else
            {
                pp = this.cache[host];
            }
            pp.Outputs[attr.Name] = pin.PluginIO;

            this.pincache.Add(pin.PluginIO, host);
            if (this.PinAdded != null) { this.PinAdded(pin,host, pin.PluginIO, TPinDirection.Output); }
            return pin;
        }
        #endregion

        #region Inputs Connected
        private void InputPinConnected(object sender, PinConnectionEventArgs args)
        {
            PropertyInfo pi = sender.GetType().GetProperty("PluginIO");
            IPluginIO io = pi.GetValue(sender, null) as IPluginIO;

            if (this.InputPinConnect != null)
            {
                viInternalLink link = new viInternalLink();
                link.Host = this.pincache[io];
                link.Instance = io;
                link.Otherpin = args.OtherPin;
                this.InputPinConnect(link);
            }
        }

        private void InputPinDisconnected(object sender, PinConnectionEventArgs args)
        {

            PropertyInfo pi = sender.GetType().GetProperty("PluginIO");
            IPluginIO io = pi.GetValue(sender, null) as IPluginIO;

            if (this.InputPinDisconnect != null)
            {
                viInternalLink link = new viInternalLink();
                link.Host = this.pincache[io];
                link.Instance = io;
                link.Otherpin = args.OtherPin;
                this.InputPinDisconnect(link);
            }
        }
        #endregion
    }
}
