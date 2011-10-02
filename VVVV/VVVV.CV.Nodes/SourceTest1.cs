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
    [StreamProcessorInfo("Source 1")]
    public class SourceTest1 : IStreamProcessor
    {
        private IStreamHost host;

        private Parameter<double> p1;

        private OutputStream<DoubleStream> testoutputstream;

        private OutputStream<ImageStream> to2;

        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            this.testoutputstream = host.CreateOutputStream<DoubleStream>("Stream Out");
            this.to2 = host.CreateOutputStream<ImageStream>("Image Out");

            this.p1 = host.CreateParameter<double>("Param 1");

        }

        public void Process()
        {
            this.testoutputstream.Data.Array = new List<double>(this.p1.Data);

            Debug.WriteLine("Source Called");
        }
        #endregion


        public void Dispose()
        {
            //Release any temp resource here
        }

    }
}
