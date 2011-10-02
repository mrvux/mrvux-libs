using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.SubGraphBuilder.Model.Actions
{
    public class AddPinGraphAction : IGraphAction
    {
        private viPin pin;
        private viGraph graph;

        public AddPinGraphAction(viPin pin)
        {
            this.pin = pin;
        }

        public void SetGraph(viGraph graph)
        {
            this.graph = graph;
        }

        public void ProcessAction()
        {
        }
    }
}
