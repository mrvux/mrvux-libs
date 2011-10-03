using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VVVV.CV.Lib.Interfaces;
using VVVV.CV.Lib.Host;
using VVVV.CV.Lib.DataTypes;
using System.Diagnostics;
using VVVV.CV.Lib.DataTypes.Streams.Data;
using System.Windows.Forms;
using System.Drawing;

namespace VVVV.CV.Nodes
{
    [StreamProcessorInfo("Window 1")]
    public class TestWindow1 : IStreamProcessor
    {
        private IStreamHost host;

        private InputStream<ImageStream> image;
        private Form frm;
        private Bitmap b;

        #region IStreamProcessor Members
        public void Init(IStreamHost host)
        {
            this.host = host;
            this.image = host.CreateInputStream<ImageStream>("Stream In");

            this.frm = new Form();
            this.frm.Show();
            frm.Paint += new PaintEventHandler(frm_Paint);           
        }

        void frm_Paint(object sender, PaintEventArgs e)
        {
            if (b != null)
            {
                e.Graphics.DrawImage(b, 0, 0, this.frm.Width, this.frm.Height);
            }
        }

        public void Process()
        {
            if (this.image.IsValid)
            {
                b = this.image.Data.Image.Bitmap;
                this.frm.Invalidate();
            }
            else
            {
                b = null;
            }

        }
        #endregion


        public void Dispose()
        {
            //Release any temp resource here
        }

    }
}
