using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLGraphService
{
    public class MultilayerCognitiveConceptState
    {
        public string Name { get; set; }

        public float Aggregate { get; set; }

        public  CognitiveConceptLayerState[] Levels { get; set; }
    }
}
