using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.CV.Lib.Interfaces;
using VVVV.CV.Lib.Host;
using VVVV.CV.Lib.DataTypes;
using System.Diagnostics;

namespace VVVV.CV.Nodes
{
    [StreamProcessorInfo("Testing 1")]
    public class ProcessorTest1 : IStreamProcessor
    {
        private IStreamHost host;

        private Parameter<double> p1;
        private Parameter<double> p2;

        private InputStream<IBaseStream> testinputstream;

        private OutputStream<IBaseStream> testoutputstream;

        private OutputData<double> testoutdata;
        


        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            this.testinputstream = host.CreateInputStream<IBaseStream>("Stream In");
            this.testoutputstream = host.CreateOutputStream<IBaseStream>("Stream Out");

            this.p1 = host.CreateParameter<double>("Param 1");
            this.p2 = host.CreateParameter<double>("Param 2");

            this.testoutdata = host.CreateOutput<double>("Out 1");
        }

        public void Process()
        {
            this.testoutdata.Data.Clear();
            this.testoutdata.Data.Add(this.p1.Data[0]);

            Debug.WriteLine("I'm called!");
            //this.p1.Data
        }
        #endregion


        public void Dispose()
        {
            //Release any temp resource here
        }

    }
}
