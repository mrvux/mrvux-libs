﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.CV.Lib.Interfaces;
using VVVV.CV.Lib.Host;
using VVVV.CV.Lib.DataTypes;
using System.Diagnostics;
using VVVV.CV.Lib.DataTypes.Streams.Data;

namespace VVVV.CV.Nodes
{
    [StreamProcessorInfo("Info")]
    public class InfoTest1 : IStreamProcessor
    {
        private IStreamHost host;

        private InputStream<ImageStream> image;

        private OutputData<int> size;

        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            this.image = host.CreateInputStream<ImageStream>("Stream In");

            this.size = host.CreateOutput<int>("Output 1");
        }

        public void Process()
        {
            if (this.image.IsValid)
            {
                this.size.Data.Clear();
                this.size.Data.Add(this.image.Data.Image.Size.Width);
                this.size.Data.Add(this.image.Data.Image.Size.Height);
            }
            else
            {
                this.size.Data.Clear();
                this.size.Data.Add(0);
                this.size.Data.Add(0);
            }

        }
        #endregion


        public void Dispose()
        {
            //Release any temp resource here
        }

    }
}
