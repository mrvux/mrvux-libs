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
    [StreamProcessorInfo("Threshold")]
    public class ThresholdFilter : IStreamProcessor
    {
        private IStreamHost host;

        private InputStream<ImageStream> image;
        private Parameter<double> thr;
        private Parameter<double> max;
        private Parameter<Emgu.CV.CvEnum.THRESH> thresholdtype;

        private OutputStream<ImageStream> outimg;

        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            this.image = host.CreateInputStream<ImageStream>("Stream In");
            this.thr = host.CreateParameter<double>("Threshold");
            this.max = host.CreateParameter<double>("Max Value");
            this.thresholdtype = host.CreateParameter<Emgu.CV.CvEnum.THRESH>("Threshold Type");

            this.outimg = host.CreateOutputStream<ImageStream>("Stream Out");
        }

        public void Process()
        {
            if (this.image.IsValid)
            {
               IImage src = this.image.Data.Image;
               IImage output = null;
                if (this.outimg.Data.IsValid)
                {
                    output = this.outimg.Data.Image;
                    if (output.Size != src.Size)
                    {
                        output.Dispose();
                        output = null;
                    }
                }

                if (output == null)
                {
                    output = new Emgu.CV.Image<Gray, byte>(src.Size.Width, src.Size.Height);

                }

                Emgu.CV.CvInvoke.cvThreshold(src.Ptr, output.Ptr,this.thr.Data[0], this.max.Data[0],this.thresholdtype.Data[0]);

                this.outimg.Data.Image = output;
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
