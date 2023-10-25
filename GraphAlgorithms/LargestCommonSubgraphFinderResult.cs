using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms
{
    public class LargestCommonSubgraphFinderResult
    {
        public Graph Graph1 { get; set; }
        public Graph Graph2 { get; set; }
        public Graph CommonSubgraph { get; set; }

        public LargestCommonSubgraphFinderResult(Graph graph1, Graph graph2, Graph commonSubgraph)
        {
            Graph1 = graph1;
            Graph2 = graph2;
            CommonSubgraph = commonSubgraph;
        }
    }
}
