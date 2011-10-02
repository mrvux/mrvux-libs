using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.SubGraphBuilder.Model
{
    public class viOutputPin : viPin
    {

        public viOutputPin(viNode parentnode) : base(parentnode) 
        {
            this.ChildrenPins = new List<viInputPin>();
            this.ParentNode.OutputPins.Add(this);
        }

        public List<viInputPin> ChildrenPins { get; private set; }

    }
}
