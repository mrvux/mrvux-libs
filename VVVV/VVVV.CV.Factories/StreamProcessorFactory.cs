using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using VVVV.Core;
using VVVV.Core.Logging;
using VVVV.Core.Model;
using VVVV.Core.Runtime;
using VVVV.Hosting;
using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Imaging.Poc.Factory;
using VVVV.CV.Lib.Interfaces;
using VVVV.CV.Lib.Host;
using VVVV.SubGraphBuilder.Builder;
using VVVV.CV.Lib.DataTypes;
using VVVV.CV.Factories.Evaluator;
using VVVV.SubGraphBuilder.Model;
using VVVV.CV.Lib.DataTypes.Streams;
using VVVV.CV.Lib.DataTypes.Streams.Data;


namespace VVVV.Hosting.Factories
{
    [Export(typeof(IAddonFactory))]
    [ComVisible(false)]
    public class StreamProcessorFactory : AbstractFileFactory<IInternalPluginHost>
    {
        //[Import]
        protected IHDEHost FHdeHost;

        [Import]
        protected DotNetPluginFactory FDNFactory;

        private viGraphBuilder2<IStreamProcessor> graphbuilder;
        
        private Dictionary<viNode, IStreamProcessor> processors = new Dictionary<viNode, IStreamProcessor>();
        private Dictionary<IPluginIO, BaseInputStream> inputstreams = new Dictionary<IPluginIO, BaseInputStream>();
        private Dictionary<IPluginIO, object> outputstreams = new Dictionary<IPluginIO, object>();
        
        private ThreadedStreamEvaluator evaluator;

        private StreamProcessorLocator filterlocator = new StreamProcessorLocator();

        //private int nodecount =0;

        #region Constructor
        [ImportingConstructor]
        public StreamProcessorFactory(CompositionContainer parentContainer,IHDEHost hdeHost)
            : this(parentContainer, ".dll")
        {
            this.graphbuilder = new viGraphBuilder2<IStreamProcessor>(hdeHost);         
            this.graphbuilder.RegisterPinType<InternalStream<ImageStream>>();
                        
            this.graphbuilder.NodeAdded += new NodeEventDelegate(graphbuilder_NodeAdded);
            this.graphbuilder.NodeRemoved += new NodeEventDelegate(graphbuilder_NodeRemoved);
            this.graphbuilder.LinkAdded += new LinkEventDelegate(graphbuilder_LinkAdded);
            this.graphbuilder.LinkRemoved += new LinkEventDelegate(graphbuilder_LinkRemoved);

            
            this.evaluator = new ThreadedStreamEvaluator();
            this.evaluator.graph = this.graphbuilder.Graph;
            this.evaluator.processors = this.processors;
            this.evaluator.Start();
          
            this.FHdeHost = hdeHost;
            //FHost.RootNode.Pins.ToArray()[0].
        }

        void graphbuilder_LinkRemoved(viLink link)
        {
            BaseInputStream bs = StreamHostNode.INPUTS[link.Sink.ComInstance];
            //object os = StreamHostNode.OUTPUTS[link.Source.ComInstance];
            bs.Disconnect();
            //bs.Disconnect();        
        }

        void graphbuilder_LinkAdded(viLink link)
        {
            BaseInputStream bs = StreamHostNode.INPUTS[link.Sink.ComInstance];
            object os = StreamHostNode.OUTPUTS[link.Source.ComInstance];
            bs.Connect(os);
        }

        void graphbuilder_NodeRemoved(VVVV.SubGraphBuilder.Model.viNode node)
        {
            this.processors.Remove(node);
        }

        void graphbuilder_NodeAdded(VVVV.SubGraphBuilder.Model.viNode node)
        {
            this.processors.Add(node, node.Instance<StreamHostNode>().Processor);
        }

        protected StreamProcessorFactory(CompositionContainer parentContainer, string fileExtension)
            : base(fileExtension)
        {

        }
        #endregion


        #region IAddonFactory

        public override string JobStdSubPath
        {
            get
            {
                return "streams";
            }
        }

        protected override void AddSubDir(string dir, bool recursive)
        {
            // Ignore obj directories used by C# IDEs
            if (dir.EndsWith(@"\obj\x86") || dir.EndsWith(@"\obj\x64")) return;

            base.AddSubDir(dir, recursive);
        }

        protected override bool CreateNode(INodeInfo nodeInfo, IInternalPluginHost pluginHost)
        {
            //dispose previous plugin
            var plugin = pluginHost.Plugin;
            if (plugin != null)
            {
                if (plugin is IDisposable)
                {
                    ((IDisposable)plugin).Dispose();
                }
            }

            //make the host mark all its pins for possible deletion
            pluginHost.Plugin = null;

            var filter = (IStreamProcessor)Activator.CreateInstance((Type)nodeInfo.UserData);

            var node = new StreamHostNode(pluginHost);
            
            node.SetProcessor(filter);
            pluginHost.Plugin = node;

            return true;
        }

        protected override bool DeleteNode(INodeInfo nodeInfo, IInternalPluginHost pluginHost)
        {
            var plugin = pluginHost.Plugin;

            if (plugin != null)
            {
                if (plugin is IDisposable)
                {
                    ((IDisposable)plugin).Dispose();
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called by AbstractFileFactory to extract all node infos in given file.
        /// </summary>
        protected override IEnumerable<INodeInfo> LoadNodeInfos(string filename)
        {
            //Try this random hack
            var nodeInfos = new List<INodeInfo>();
            LoadNodeInfosFromFile(filename, filename, ref nodeInfos, true);
            return nodeInfos;
        }

        protected void LoadNodeInfosFromFile(string filename, string sourcefilename, ref List<INodeInfo> nodeInfos, bool commitUpdates)
        {
            var assembly = Assembly.LoadFrom(filename);

            this.filterlocator.Scan(assembly);

            foreach (string key in this.filterlocator.RegisteredTypes.Keys)
            {
                Type t = this.filterlocator.RegisteredTypes[key];
                var nodeInfo = FNodeInfoFactory.CreateNodeInfo(key, "Imaging", String.Empty, filename, true);
                nodeInfo.Arguments = Assembly.GetExecutingAssembly().Location.ToLower();
                nodeInfo.Factory = this;
                nodeInfo.Type = NodeType.Plugin;
                nodeInfo.UserData = t;
                nodeInfo.CommitUpdate();
                nodeInfo.UserData = t;
                nodeInfo.AutoEvaluate = true;
                nodeInfos.Add(nodeInfo);
            }

            foreach (var nodeInfo in nodeInfos)
            {
                nodeInfo.Factory = this;
                if (commitUpdates)
                    nodeInfo.CommitUpdate();
            }
        }
        #endregion


        protected virtual string GetAssemblyLocation(INodeInfo nodeInfo)
        {
            return nodeInfo.Filename;
        }





    }
}