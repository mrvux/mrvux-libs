using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.CV.Lib.DataTypes.Streams.Data
{
    public class DoubleStream : IBaseStream
    {
        public List<double> Array = new List<double>();

        public bool IsValid
        {
            get { return this.Array != null; }
        }

        public void Dispose()
        {
            
        }
    }
}
