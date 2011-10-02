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
    [StreamProcessorInfo("Info 1")]
    public class InfoTest1 : IStreamProcessor
    {
        private IStreamHost host;

        private InputStream<ImageStream> image;

        private OutputData<int> width;

        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            this.image = host.CreateInputStream<ImageStream>("Stream In");

            this.width = host.CreateOutput<int>("Output 1");
        }

        public void Process()
        {
            if (this.image.IsValid)
            {
               // Emgu.CV.Image<Emgu.CV.Structure.Bgr,Byte> img ;//= new Emgu.CV.Image<Emgu.CV.Structure.Bgr,byte>(
                
                //img.
                //this.image.Data.Image
                this.width.Data.Clear();
                this.width.Data.Add(this.image.Data.Image.Size.Width);
                this.width.Data.Add(this.image.Data.Image.Size.Height);
            }
            else
            {
                this.width.Data.Clear();
                this.width.Data.Add(0);
                this.width.Data.Add(0);
            }

        }
        #endregion


        public void Dispose()
        {
            //Release any temp resource here
        }

    }
}
