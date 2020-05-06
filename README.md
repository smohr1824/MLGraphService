# MLGraphService
This is a REST API utilizing the C# Graphs library to provide multilayer network services to client user interfaces.  
This is currently pre-production code to support the Angular graph-composer project.

## Operations

### POST
**URL**: api/multilayer/fcm/{name}

**name**: name of FCM

**Body**: GML syntax network definition 

Adds a new multilayer to the server's collection of ML FCMs. On success, the map is available for
the remaining operations.

### PUT
**URL**: api/multilayer/fcm/{name}

***name***: name of FCM

**Body**: GML syntax network definitio

Updates an existing ML FCM in the server's collection. The map remains available for 
the remaining operations.

### GET
**URL**: api/multilayer/fcm/{name}?generations={steps}

***name***: name of FCM

***steps***: number of generations to execute

Resets the network to its initial state, then steps through `steps` of network execution. A successful
response consists of an array of `MultilayerCognitiveConceptState` objects is returned. The array is ordered
by generation, and within each generation by concept.

### DELETE
**URL**: api/multilayer/fcm/{name}

**name**: name of FCM

Removes a previously defined map from the server's collecton. The map is no longer available for other
operations.

## Pre-production Limitations
1. The server maintains the collection of maps in memory.  There is no persistent server-side storage.
2. CORS protections are disabled to allow for separate hosting of the client and the REST API.
3. There is no secure identification of users and access control. JWT authentication is planned.

