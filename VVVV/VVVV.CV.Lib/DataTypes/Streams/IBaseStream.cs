using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.CV.Lib.DataTypes
{
    /// <summary>
    /// Base Interface for any stream
    /// </summary>
    public interface IBaseStream : IDisposable
    {
        bool IsValid { get; }
        
    }
}
