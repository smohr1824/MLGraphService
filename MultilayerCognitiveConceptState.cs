using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Networks.FCM;

namespace MLGraphService
{
    public class MultilayerCognitiveConceptState
    {
        public string Name { get; set; }
        public int Generation { get; set; }
        public float Aggregate { get; set; }

        public IEnumerable<ILayerActivationLevel> Levels { get; set; }
    }

}
