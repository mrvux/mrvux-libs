using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.PluginInterfaces.V2;
using VVVV.CV.Lib.Interfaces;
using VVVV.CV.Lib.DataTypes;
using VVVV.PluginInterfaces.V1;
using VVVV.Hosting.Pins;
using VVVV.CV.Lib.DataTypes.Streams;

namespace VVVV.CV.Lib.Host
{
    public delegate void InputStreamDelegate(IPluginIO pinio,BaseInputStream stream);
    public delegate void OutputStreamDelegate(IPluginIO pinio, object stream);

    public class StreamHostNode : IPluginEvaluate, IStreamHost
    {
        public static Dictionary<IPluginIO, BaseInputStream> INPUTS = new Dictionary<IPluginIO, BaseInputStream>();
        public static Dictionary<IPluginIO, object> OUTPUTS = new Dictionary<IPluginIO, object>();

        protected IStreamProcessor processor;
        protected IPluginHost host;

        protected List<IParameter> paramlist = new List<IParameter>();
        protected List<IOutputData> outputlist = new List<IOutputData>();

        public event InputStreamDelegate InputStreamCreated;
        public event OutputStreamDelegate OutputStreamCreated;

        public StreamHostNode(IPluginHost host)
        {
            this.host = host;
        }

        public IStreamProcessor Processor
        {
            get { return this.processor; }
        }

        public void SetProcessor(IStreamProcessor processor)
        {
            this.processor = processor;
            this.processor.Init(this);
        }

        #region Evaluate
        public void Evaluate(int SpreadMax)
        {
            //Cache all parameters
            foreach (IParameter p in this.paramlist)
            {
                p.CacheData();
            }

            //Get Back all outputs
            foreach (IOutputData o in this.outputlist)
            {
                o.CopyOutput();
            }
        }
        #endregion

        #region IStreamHost Members
        public Parameter<T> CreateParameter<T>(string name)
        {
            Parameter<T> p = new Parameter<T>(this.host, name);
            this.paramlist.Add(p);
            return p;
        }

        public OutputData<T> CreateOutput<T>(string name)
        {
            OutputData<T> o = new OutputData<T>(this.host,name);
            this.outputlist.Add(o);
            return o;
        }

        public InputStream<T> CreateInputStream<T>(string name) where T : IBaseStream,new()
        {
            InputAttribute attr = new InputAttribute(name);

            Pin<InternalStream<T>> pin = PinFactory.CreatePin<InternalStream<T>>(this.host, attr);
            InputStream<T> stream = new InputStream<T>();

            INPUTS.Add(pin.PluginIO, stream);

            if (this.InputStreamCreated != null) { this.InputStreamCreated(pin.PluginIO,stream); }

            return stream;
        }

        public OutputStream<T> CreateOutputStream<T>(string name) where T : IBaseStream,new()
        {
            OutputAttribute attr = new OutputAttribute(name);

            Pin<InternalStream<T>> pin = PinFactory.CreatePin<InternalStream<T>>(this.host, attr);

            OutputStream<T> stream = new OutputStream<T>();

            OUTPUTS.Add(pin.PluginIO, stream);

            if (this.OutputStreamCreated != null) { this.OutputStreamCreated(pin.PluginIO, stream); }

            return stream;
        }
        #endregion
    }
}
