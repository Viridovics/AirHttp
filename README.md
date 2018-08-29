# AirHttp

AirHttp is lightweight http library for .NET

## Support features

* GET, POST, PUT, DELETE, HEAD, PATCH
* Support async programming model
* Server-side coookie
* Support any serialization/deserialization framework for JSON, XML, etc
* .NET Framework 4.5 and netstandard 2.0 assemblies
* Rest client for WebApi 2 controller
* Fluent URL builder

## Installation

* Download sources and compile
* Use [http://nuget.org](http://nuget.org)

Nuget CLI command:

```
Install-Package AirHttp 
```

## Simple start

### Use default json content processor

```
using AirHttp.ContentProcessors;

var contentProcessor = new SimpleJsonContentProcessor(); // By default process json field names in case sensitive mode. Support [DataContract]/[DataMember] attributes.

or

var contentProcessor = new WeakJsonContentProcessor(); // Process json field names in case insensitive mode. Warning: if one field have name "name" and second field have name "NAME" then one of fields will not processed.

```

### Or create custom content processor

[Example processor](https://github.com/Viridovics/AirHttp/blob/master/src/AirHttp.NewtonsoftJson/Configuration/NewtonsoftJsonAirContentProcessor.cs) uses Json library ([Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json))
```
using Newtonsoft.Json;
using AirHttp.Configuration;
using AirHttp.Protocols;

namespace YourProjectNamespace
{
    public class NewtonsoftJsonAirContentProcessor : IAirContentProcessor
    {
        public string ContentType => ContentTypes.Json;

        public T DeserializeObject<T>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        public string SerializeObject<T>(T pocoObject)
        {
            return JsonConvert.SerializeObject(pocoObject);
        }
    }
}


var contentProcessor = new NewtonsoftJsonAirContentProcessor();
```

### Create AirHttpClient
```
using AirHttp.Client;

var airClient = new AirHttpClient(contentProcessor); //With default parameters

// If you want to change default http parameters then use HttpClientParameters/IHttpClientParameters
using AirHttp.Configuration;

var airClient = new AirHttpClient(contentProcessor,
                                    new HttpClientParameters
                                    {
                                        TimeoutInMilliseconds = 2000
                                    });
```
### Get

```
var getResult = airClient.Get<ObjectFromServer>(@"url");
if (getResult.Failed)
{
    //Process error getResult.FaultException
}
var objFromServer = getResult.Value;
Console.WriteLine(objFromServer.Id);
Console.WriteLine(objFromServer.Name);
// etc
```

### Post

```
var objectForPostBody = new ObjectToServer(...) {...};
var getResult = airClient.Post<ObjectToServer, ObjectFromServer>(@"url", objectForPostBody);
var objFromServer = getResult.Value;
Console.WriteLine(objFromServer.Id);
Console.WriteLine(objFromServer.Name);
// etc
```

### Deferred style (method chaining)

```
using AirHttp.Responses.DefferedExtensions;

var getResult = airClient.Get<ObjectFromServer>(@"url");
getResult.Fail(e => //Process error)
         .Success(val => 
                        {
                            Console.WriteLine(val.Id);
                            Console.WriteLine(val.Name);
                            // etc
                        });
```

### Simple REST api client

```
using AirHttp.Client.Rest

var restClient = new AirRestClient<IdType, CollectionObject>(@"http://domain/api/controller", new WeakJsonContentProcessor());
restClient.Get() // Get all collection
restClient.Get(id) // Get object by id/key
restClient.Post(object) // Post new object
restClient.Put(id, object) // Put object
restClient.Delete(id) // Delete object by id/key
```

### Building uri in fluent style

```
using AirHttp.UriFluentBuilder.Extensions;

var uriWithSegments = "localhost:52870".AddHttp().AddSegment("api").AddSegment("rest");
//uriWithSegments is 'http://localhost:52870/api/rest'
var uriWithQuery = "localhost".AddPort(8080).AddHttps().AddWWW().AddQueryParams(new { id = 5 })
                                                        .AddQueryParam("point", "42");
//uriWithQuery is 'https://www.localhost:8080?id=5&point=42'
```

# License
Copyright (c) Viridovics

Licensed under the [MIT](LICENSE) License.

# Credits
AirHttp library uses [SimpleJson.cs](https://github.com/facebook-csharp-sdk/simple-json/blob/master/src/SimpleJson/SimpleJson.cs) under [MIT License](https://github.com/facebook-csharp-sdk/simple-json/blob/master/LICENSE.txt)

AirHttp example uses Json library ([Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)) under [MIT License](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)