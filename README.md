# AirHttp

AirHttp is lightweight http library for .NET

## Support features

* GET, POST, PUT, DELETE, HEAD, PATCH, OPTIONS
* Support async programming model
* Server-side coookie
* Selection any serialization/deserialization framework for JSON, XML, etc
* .NET Framework 4.5 and netstandard 2.0 assemblies

## Installation

* Download sources and compile
* Use [http://nuget.org](http://nuget.org)

Nuget CLI command:

```
Install-Package AirHttp 
```

## Simple start

### Create custom content processor

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
```

### Create AirHttpClient
```
using AirHttp.Client;

var airClient = new AirHttpClient(new NewtonsoftJsonAirContentProcessor());
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

# License
Copyright (c) Viridovics

Licensed under the [MIT](LICENSE) License.

# Credits
AirHttp example uses Json library ([Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)) under [MIT License](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)