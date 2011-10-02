using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.SubGraphBuilder.Model
{
    public class viInputPin : viPin
    {
        public viInputPin(viNode parentnode) : base(parentnode) 
        {
            this.ParentNode.InputPins.Add(this);
        }

        public bool IsConnected { get { return this.ParentPin != null; } }

        public viOutputPin ParentPin { get; set; }

        public void Disconnect(viOutputPin op)
        {
            if (op == this.ParentPin)
            {
                op.ChildrenPins.Remove(this);
                this.ParentPin = null;
            }
        }

        public void Connect(viOutputPin op)
        {
            if (this.ParentPin == null)
            {
                op.ChildrenPins.Add(this);
                this.ParentPin = op;
            }
        }


    }
}
