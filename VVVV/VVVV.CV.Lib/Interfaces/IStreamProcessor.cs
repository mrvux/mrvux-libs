using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.CV.Lib.Host;

namespace VVVV.CV.Lib.Interfaces
{
    public class StreamProcessorInfoAttribute : Attribute
    {
        public StreamProcessorInfoAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }

    public interface IStreamProcessor : IDisposable
    {
        void Init(IStreamHost host);
        void Process();
    }
}
