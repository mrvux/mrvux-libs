using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.CV.Lib.Interfaces;
using VVVV.CV.Lib.Host;
using VVVV.CV.Lib.DataTypes;
using System.Diagnostics;
using VVVV.CV.Lib.DataTypes.Streams.Data;

namespace VVVV.CV.Nodes
{
    [StreamProcessorInfo("Sink 1")]
    public class SinkTest1 : IStreamProcessor
    {
        private IStreamHost host;

        private InputStream<DoubleStream> i1;

        private OutputData<double> o1;

        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            this.i1 = host.CreateInputStream<DoubleStream>("Stream In");

            this.o1 = host.CreateOutput<double>("Output 1");
        }

        public void Process()
        {
            Debug.WriteLine("Sink Called");

            this.o1.Data.Clear();

            if (this.i1.IsValid)
            {
                this.o1.Data.AddRange(this.i1.Data.Array);
            }

            
        }
        #endregion


        public void Dispose()
        {
            //Release any temp resource here
        }

    }
}
