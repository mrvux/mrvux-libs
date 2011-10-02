using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.SubGraphBuilder.Model;
using VVVV.SubGraphBuilder.Listeners;
using VVVV.PluginInterfaces.V2.Graph;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using System.Diagnostics;

namespace VVVV.SubGraphBuilder.Builder
{
    public delegate void NodeEventDelegate(viNode node);
    public delegate void LinkEventDelegate(viLink link);

    public class viGraphBuilder2<N> : AbstractHdeNodeListener
    {

        private viGraph graph;
        public event NodeEventDelegate NodeAdded;
        public event NodeEventDelegate NodeRemoved;

        public event LinkEventDelegate LinkAdded;
        public event LinkEventDelegate LinkRemoved;

        //Cache the host->node reference
        private Dictionary<IPluginHost, viNode> hostnodes;

        //List of pending links (since i can receive link info before node added)
        private List<viInternalLink> pendinglinks = new List<viInternalLink>();

        private PinInstanceListener pinlistener;

        public viGraph Graph { get { return this.graph; } }

        public viGraphBuilder2(IHDEHost hde) : base(hde)
        {
            this.graph = new viGraph();

            this.pinlistener = new PinInstanceListener();
            this.pinlistener.InputPinConnect += pinlistener_InputPinConnect;
            this.pinlistener.InputPinDisconnect += pinlistener_InputPinDisconnect;
            this.hostnodes = new Dictionary<IPluginHost, viNode>();
        }
      
        #region Register Pin Type
        /// <summary>
        /// Registers a Pin Type to listen instances from
        /// </summary>
        /// <typeparam name="T">Type to listen</typeparam>
        public void RegisterPinType<T>()
        {
            this.pinlistener.RegisterType<T>();
        }
        #endregion

        #region Process Added Node
        protected override bool ProcessAddedNode(INode2 node)
        {
            
            if (node.IsNodeAssignableFrom<N>())
            {
                viNode vn = new viNode();
                vn.HdeNode = node.InternalCOMInterf;
                vn.Hoster = (IPluginHost)node.InternalCOMInterf;

                this.hostnodes.Add(vn.Hoster, vn);
                this.graph.Nodes.Add(vn);
                

                Debug.WriteLine("Added Node: " + node.Name);

                foreach (IPin2 p in node.Pins)
                {
                    this.ProcessAddedPin(p);
                }

                this.ProcessPendingLinks();

                if (this.NodeAdded != null) { this.NodeAdded(vn); }

                return true;
            }
            return false;
        }
        #endregion

        #region Process Removed Node
        protected override bool ProcessRemovedNode(INode2 node)
        {
            viNode vn = this.graph.FindNode(node);

            if (vn != null)
            {
                Debug.WriteLine("Removed Node: " + node.Name);

                foreach (IPin2 pin in node.Pins)
                {
                    this.ProcessRemovedPin(pin);
                }

                this.hostnodes.Remove(vn.Hoster);
                this.graph.Nodes.Remove(vn);

                if (this.NodeRemoved != null) { this.NodeRemoved(vn); }

                return true;
            }
            return false;
        }
        #endregion

        #region Process Added Pin
        protected override bool ProcessAddedPin(IPin2 pin)
        {
            viNode vn = this.graph.FindNode(pin.ParentNode);

            if (vn != null)
            {

                IPluginIO ip = this.pinlistener.GetInput(vn.Hoster, pin.Name);
                if (ip != null)
                {
                    viInputPin vip = new viInputPin(vn);
                    vip.ComInstance = ip;
                    vip.Name = pin.Name;
                    vip.HdePin = vn.HdeNode.FindHdePinByName(pin.Name);


                    Debug.WriteLine("Added Input Pin: " + vip.Name);
                }
                else
                {
                    IPluginIO op = this.pinlistener.GetOutput(vn.Hoster, pin.Name);
                    if (op != null)
                    {
                        viOutputPin vop = new viOutputPin(vn);
                        vop.ComInstance = op;
                        vop.Name = pin.Name;
                        vop.HdePin = vn.HdeNode.FindHdePinByName(pin.Name);

                        Debug.WriteLine("Added Output Pin: " + vop.Name);
                    }
                }
            }

            return false;
        }
        #endregion

        #region Process Removed Pin
        protected override bool ProcessRemovedPin(IPin2 pin)
        {
            viNode vn = this.graph.FindNode(pin.ParentNode);

            if (vn != null)
            {
                //Remove from cache
                this.pinlistener.RemovePin(vn.Hoster, pin.Name);
                vn.RemovePin(pin.Name);

            }

            return false;
        }
        #endregion

        #region On Pin Disconnected
        void pinlistener_InputPinDisconnect(viInternalLink link)
        {
            viPin sourcepin = this.graph.FindPin(link.Otherpin);

            viOutputPin op = (viOutputPin)sourcepin;

            viNode sink = this.hostnodes[link.Host];
            viInputPin ip = null;
            foreach (viInputPin i in sink.InputPins)
            {
                if (i.ComInstance == link.Instance) { ip = i; }
            }

            ip.Disconnect(op);

            if (this.LinkRemoved != null)
            {
                viLink l = new viLink();
                l.Sink = ip;
                l.Source = op;
                this.LinkRemoved(l);
            }

            Debug.WriteLine("Disconnect Source: " + sourcepin.ParentNode.HdeNode.GetID().ToString() + " Sink: " + sink.HdeNode.GetID());

        }
        #endregion

        #region On Pin Connected
        private void pinlistener_InputPinConnect(viInternalLink link)
        {
            //Try to process the link, if it fails 
            //(due to info not available yet, place in queue)
            if (!this.ProcessLink(link))
            {
                this.pendinglinks.Add(link);
            }
        }
        #endregion

        #region Process Link
        private bool ProcessLink(viInternalLink link)
        {
            viPin sourcepin = this.graph.FindPin(link.Otherpin);
            viOutputPin op = (viOutputPin)sourcepin;

            if (sourcepin != null && this.hostnodes.ContainsKey(link.Host))
            {
                viNode sink = this.hostnodes[link.Host];

                viInputPin ip = null;
                foreach (viInputPin i in sink.InputPins)
                {
                    if (i.ComInstance == link.Instance) { ip = i; }
                }

                ip.Connect(op);
                Debug.WriteLine("Connect Source: " + sourcepin.ParentNode.HdeNode.GetID().ToString() + " Sink: " + sink.HdeNode.GetID());

                if (this.LinkAdded != null)
                {
                    viLink l = new viLink();
                    l.Sink = ip;
                    l.Source = op;
                    this.LinkAdded(l);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Process Pending Links
        private void ProcessPendingLinks()
        {
            //Try to process all links still in pending list
            List<viInternalLink> np = new List<viInternalLink>();
            foreach (viInternalLink p in this.pendinglinks)
            {
                if (!this.ProcessLink(p))
                {
                    np.Add(p);
                }
            }
            this.pendinglinks = np;
        }
        #endregion
    }
}
