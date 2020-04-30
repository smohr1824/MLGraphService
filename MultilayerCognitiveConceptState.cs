using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Networks.FCM;

namespace MLGraphService
{
    public class MultilayerCognitiveConceptState
    {
        public float Aggregate { get; set; }

        public IEnumerable<ILayerActivationLevel> Levels { get; set; }
    }
}
