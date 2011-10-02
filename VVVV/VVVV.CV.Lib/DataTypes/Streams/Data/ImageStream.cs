using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;

namespace VVVV.CV.Lib.DataTypes.Streams.Data
{
    public class ImageStream : IBaseStream
    {
        public IImage Image;
        public bool IsValid
        {
            get { return this.Image != null; }
        }

        public void Dispose()
        {
            
        }
    }
}
