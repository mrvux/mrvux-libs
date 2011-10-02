using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.CV.Lib.DataTypes;

namespace VVVV.CV.Lib.Host
{
    public interface IStreamHost
    {
        InputStream<T> CreateInputStream<T>(string name) where T : IBaseStream, new();

        OutputStream<T> CreateOutputStream<T>(string name) where T : IBaseStream, new();

        Parameter<T> CreateParameter<T>(string name);

        OutputData<T> CreateOutput<T>(string name);
    }
}
