using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Networks.Core;
using Networks.FCM;

namespace MLGraphService.Controllers
{
    [ApiController]
    [Route("api/Multilayer/FCM")]
    public class MultilayerFCMController : ControllerBase
    {

        private Dictionary<string, MultilayerFuzzyCognitiveMap> map = new Dictionary<string, MultilayerFuzzyCognitiveMap>();


        [HttpGet("{name}/execute")]
        public ActionResult<IEnumerable<MultilayerCognitiveConceptState>> Execute(string name, int generations, string[] concepts)
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

            if (!CheckNames(concepts, fcm))
                return BadRequest("One or more requested concepts are not in the map.");
            fcm.Reset();

            // iterate and return results
            for (int i = 0; i < generations; i++)
            {
                fcm.StepWalk();
                // report the desired concepts and their levels
            }
            
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
                return BadRequest("The request body mus tcontain an extended GML document describing a multilayer FCM");
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

    }
}