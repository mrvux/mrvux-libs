using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.SubGraphBuilder.Model.Actions
{
    /// <summary>
    /// Action to be performed on a graph
    /// </summary>
    public interface IGraphAction
    {
        void SetGraph(viGraph graph);
        void ProcessAction();
    }
}
