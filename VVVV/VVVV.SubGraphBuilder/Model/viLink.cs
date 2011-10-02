using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.SubGraphBuilder.Model
{
    public class viLink
    {
        public viInputPin Sink { get; set; }
        public viOutputPin Source { get; set; }
    }
}
