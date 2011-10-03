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
using Emgu.CV;

namespace VVVV.CV.Nodes
{
    [StreamProcessorInfo("GrayScale")]
    public class GrayScaleFilter : IStreamProcessor
    {
        private IStreamHost host;

        private InputStream<ImageStream> image;

        private OutputStream<ImageStream> outimg;

        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            this.image = host.CreateInputStream<ImageStream>("Stream In");

            this.outimg = host.CreateOutputStream<ImageStream>("Stream Out");
        }

        public void Process()
        {
            if (this.outimg.Data.IsValid)
            {
                this.outimg.Data.Image.Dispose();
            }

            if (this.image.IsValid)
            {
               IImage src = this.image.Data.Image;

               Emgu.CV.Image<Gray,Byte> g = new Emgu.CV.Image<Gray,byte>(src.Size.Width,src.Size.Height);
               Emgu.CV.CvInvoke.cvCvtColor(src.Ptr, g.Ptr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
               this.outimg.Data.Image = g;
            }
            else
            {
                this.outimg.Data.Image = null;
            }
        }
        #endregion


        public void Dispose()
        {
            //Release any temp resource here
        }

    }
}
