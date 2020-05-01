using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Networks.FCM;

namespace MLGraphService
{
    public class MultilayerCognitiveConceptState
    {
        public int Generation { get; set; }
        public float Aggregate { get; set; }

        public IEnumerable<ILayerActivationLevel> Levels { get; set; }
    }

    public class MultilayerCognitiveConceptStateVector
    {
        public string Name { get; set; }
        public MultilayerCognitiveConceptState[] Generations { get; set; }
    }
}
