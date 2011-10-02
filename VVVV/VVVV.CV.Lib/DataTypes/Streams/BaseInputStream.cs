using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.CV.Lib.DataTypes.Streams
{
    public abstract class BaseInputStream
    {
        public abstract void Connect(object source);
        public abstract void Disconnect();
    }
}
