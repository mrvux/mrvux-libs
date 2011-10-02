using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.SubGraphBuilder.Model.Actions
{
    public class AddNodeGraphAction : IGraphAction
    {
        private viNode node;
        private viGraph graph;

        public AddNodeGraphAction(viNode node)
        {
            this.node = node;
        }

        public void SetGraph(viGraph graph)
        {
            this.graph = graph;
        }

        public void ProcessAction()
        {
            this.graph.Nodes.Add(node);
        }
    }

    public class RemoveNodeGraphAction : IGraphAction
    {
        private viNode node;
        private viGraph graph;

        public RemoveNodeGraphAction(viNode node)
        {
            this.node = node;
        }

        public void SetGraph(viGraph graph)
        {
            this.graph = graph;
        }

        public void ProcessAction()
        {
            this.graph.Nodes.Remove(node);
        }
    }
}
