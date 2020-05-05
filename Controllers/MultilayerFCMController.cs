using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Networks.Core;
using Networks.FCM;

namespace MLGraphService.Controllers
{
    [ApiController]
    [EnableCors("DevPolicy")]    // for testing
    [Route("api/Multilayer/FCM")]
    public class MultilayerFCMController : ControllerBase
    {

        private static Dictionary<string, MultilayerFuzzyCognitiveMap> map = new Dictionary<string, MultilayerFuzzyCognitiveMap>();


        [HttpGet("{name}/execute")]
        [EnableCors("DevPolicy")]
        public ActionResult<IEnumerable<MultilayerCognitiveConceptState>> Execute(string name, int generations = 1)
        {
            if (name == null || name == string.Empty)
                return BadRequest("Name cannot be null or empty.");

            if (generations < 1)
                return BadRequest("Request must specify a positive, nonzero number of generations.");

            MultilayerFuzzyCognitiveMap fcm;
            try
            {
                fcm = map[name];
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            fcm.Reset();
            MultilayerCognitiveConceptState[] results = new MultilayerCognitiveConceptState[fcm.Concepts.Count() * generations];

            int k = 0;
            // iterate and return results
            for (int i = 0; i < generations; i++)
            {
                fcm.StepWalk();
                // report the desired concepts and their levels

                foreach (MultilayerCognitiveConcept cog in fcm.Concepts.Values)
                {
                    //MultilayerCognitiveConceptStateVector vec = results[k++];
                    results[k++] = MakeStateFromCognitiveConcept(cog, i + 1);
                }
                
            }

            return results;
            
        }
        // POST: api/Multilayer/FCM
        [HttpPost("{name}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Post(string name)
        {
            if (name == null || name == string.Empty)
                return BadRequest("Name cannot be null or empty.");

            StreamReader reader;
            string body;
            StringReader rdr;
            try
            {
                // TODO: Fix MLFCMSerializer to do async reads
                reader = new StreamReader(Request.Body);
                body = await reader.ReadToEndAsync();
                reader.Close();
                rdr = new StringReader(body);
            }
            catch(ArgumentNullException)
            {
                return BadRequest("The request body must contain an extended GML document describing a multilayer FCM");
            }

            MultilayerFuzzyCognitiveMap fcm;
            try
            {
                fcm = MLFCMSerializer.ReadNetwork(rdr);
            }
            catch(NetworkSerializationException ex)
            {
                return BadRequest(ex.NetworkMessage);
            }
            catch(MLNetworkSerializationException mx)
            {
                return BadRequest(mx.MLNetworkMessage);
            }

            if (map.ContainsKey(name))
            {
                return BadRequest($"Network {name} is already available. Consider PUT to modify the network.");
            } else
            {
                map.Add(name, fcm);
                return Ok();
            }

        }

        private bool CheckNames(string[] names, MultilayerFuzzyCognitiveMap fcm)
        {
            List<string> mapConcepts = fcm.ListConcepts();
            foreach(string name in names)
            {
                if (!mapConcepts.Contains(name))
                    return false;
            }

            return true;
        }

        private MultilayerCognitiveConceptState MakeStateFromCognitiveConcept(MultilayerFuzzyCognitiveMap fcm, string conceptName, int generation)
        {
            MultilayerCognitiveConceptState state = new MultilayerCognitiveConceptState();
            MultilayerCognitiveConcept cog = fcm.GetConcept(conceptName);
            state.Name = cog.Name;
            state.Generation = generation;
            state.Aggregate = cog.ActivationLevel;
            state.Levels = cog.LayerActivationLevels;
            return state;
        }

        private MultilayerCognitiveConceptState MakeStateFromCognitiveConcept(MultilayerCognitiveConcept concept, int generation)
        {
            MultilayerCognitiveConceptState state = new MultilayerCognitiveConceptState();
            state.Name = concept.Name;
            state.Generation = generation;
            state.Aggregate = concept.ActivationLevel;
            state.Levels = concept.LayerActivationLevels;
            return state;
        }

    }
}