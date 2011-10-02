using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.CV.Lib.Interfaces;
using VVVV.CV.Lib.Host;
using VVVV.CV.Lib.DataTypes;
using System.Diagnostics;
using VVVV.CV.Lib.DataTypes.Streams.Data;
using Emgu.CV.Structure;

namespace VVVV.CV.Nodes
{
    [StreamProcessorInfo("File")]
    public class FileSourceTest1 : IStreamProcessor
    {
        private IStreamHost host;

        private Parameter<string> path;
        private string oldpath = String.Empty;
        
        private OutputStream<ImageStream> to;

        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            
            this.path = host.CreateParameter<string>("Path");

            this.to = host.CreateOutputStream<ImageStream>("Image Out");

        }

        public void Process()
        {
            if (oldpath != this.path.Data[0])
            {
                if (this.to.Data.Image != null)
                {
                    this.to.Data.Image.Dispose();
                }

                try
                {
                    this.to.Data.Image = new Emgu.CV.Image<Gray, Byte>(this.path.Data[0]);
                }
                catch
                {
                    this.to.Data.Image = null;
                }
            }
            this.oldpath = this.path.Data[0];
        }
        #endregion


        public void Dispose()
        {
            //Release any temp resource here
        }

    }
}
