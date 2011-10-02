using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.SubGraphBuilder.Model.Actions
{
    public class ConnectLinkGraphAction
    {
        public class AddNodeGraphAction : IGraphAction
        {
            private viLink link;
            private viGraph graph;

            public AddNodeGraphAction(viLink link)
            {
                this.link = link;
            }

            public void SetGraph(viGraph graph)
            {
                this.graph = graph;
            }

            public void ProcessAction()
            {
                link.Sink.Connect(link.Source);
            }
        }

        public class RemoveNodeGraphAction : IGraphAction
        {
            private viLink link;
            private viGraph graph;

            public RemoveNodeGraphAction(viLink link)
            {
                this.link = link;
            }

            public void SetGraph(viGraph graph)
            {
                this.graph = graph;
            }

            public void ProcessAction()
            {
                link.Sink.Disconnect(link.Source);
            }
        }
    }
}
