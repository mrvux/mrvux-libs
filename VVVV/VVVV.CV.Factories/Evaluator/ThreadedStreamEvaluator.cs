using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VVVV.SubGraphBuilder.Model;
using VVVV.CV.Lib.Host;
using VVVV.CV.Lib.Interfaces;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace VVVV.CV.Factories.Evaluator
{
    public class ThreadedStreamEvaluator
    {
        private Thread thr;
        private bool m_brunning = false;

        public viGraph graph;
        public Dictionary<viNode, IStreamProcessor> processors = new Dictionary<viNode, IStreamProcessor>();

        private List<viNode> processednodes = new List<viNode>();

        public void Start()
        {
            if (!this.m_brunning)
            {
                thr = new Thread(new ThreadStart(this.Run));
                this.m_brunning = true;
                thr.Start();
            }
        }

        private void Run()
        {
            while (m_brunning)
            {
                Debug.WriteLine("Process Frame");
                if (this.graph != null)
                {
                    this.processednodes.Clear();
                    List<viNode> nodes = new List<viNode>(this.graph.Nodes);
                    
                    foreach (viNode node in nodes)
                    {
                        this.ProcessNode(node);
                    }
                }
                Thread.Sleep(10);
            }
            
        }

        private void ProcessNode(viNode node)
        {
            //Change that later
            if (!this.processednodes.Contains(node))
            {
                if (this.processors.ContainsKey(node))
                {
                    foreach (viInputPin ip in node.InputPins)
                    {
                        if (ip.IsConnected)
                        {
                            this.ProcessNode(ip.ParentPin.ParentNode);
                        }
                    }

                    this.processors[node].Process();
                }
                this.processednodes.Add(node);
            }
            
        }

        public void Test()
        {
            if (this.graph != null)
            {
                List<viNode> nodes = new List<viNode>(this.graph.Nodes);

                foreach (viNode node in nodes)
                {
                    StreamHostNode host = node.Instance<StreamHostNode>();
                    host.Processor.Process();
                }
            }
        }

        public void Stop()
        {
            this.m_brunning = false;
        }
    }
}
