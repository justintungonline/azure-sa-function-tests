using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LocalFunctionProj
{
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            /* Begin Tutorial Code */

            var connection = ConnectionMultiplexer.Connect("<your Azure Cache for Redis connection string goes here>");
            log.LogInformation($"Connection string.. {connection}");

            // Connection refers to a property that returns a ConnectionMultiplexer
            IDatabase db = connection.GetDatabase();
            log.LogInformation($"Created database {db}");

            log.LogInformation($"Message Count {data.Count}");

            // Perform cache operations using the cache object. For example, the following code block adds few integral data types to the cache
            for (var i = 0; i < data.Count; i++)
            {
                string time = data[i].time;
                string callingnum1 = data[i].callingnum1;
                string key = time + " - " + callingnum1;
                db.StringSet(key, data[i].ToString());
                log.LogInformation($"Object put in database. Key is {key} and value is {data[i].ToString()}");

                // Simple get of data types from the cache
                string value = db.StringGet(key);
                log.LogInformation($"Database got: {value}");
            }

            /* End Tutorial Code */

            // name = name ?? data?.name;
            // string responseMessage = string.IsNullOrEmpty(name)
            //     ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //     : $"Hello, {name}. This HTTP triggered function executed successfully.";

            string responseMessage = "This HTTP triggered function executed successfully.";
            return new OkObjectResult(responseMessage);

            /* New Code

                log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

                // Get the request body
                dynamic dataArray = await req.Content.ReadAsAsync<object>();

                // Throw an HTTP Request Entity Too Large exception when the incoming batch(dataArray) is greater than 256 KB. Make sure that the size value is consistent with the value entered in the Stream Analytics portal.

                if (dataArray.ToString().Length > 262144)
                {
                return new HttpResponseMessage(HttpStatusCode.RequestEntityTooLarge);
                }
                var connection = ConnectionMultiplexer.Connect("<your Azure Cache for Redis connection string goes here>");
                log.Info($"Connection string.. {connection}");

                // Connection refers to a property that returns a ConnectionMultiplexer
                IDatabase db = connection.GetDatabase();
                log.Info($"Created database {db}");

                log.Info($"Message Count {dataArray.Count}");

                // Perform cache operations using the cache object. For example, the following code block adds few integral data types to the cache
                for (var i = 0; i < dataArray.Count; i++)
                {
                string time = dataArray[i].time;
                string callingnum1 = dataArray[i].callingnum1;
                string key = time + " - " + callingnum1;
                db.StringSet(key, dataArray[i].ToString());
                log.Info($"Object put in database. Key is {key} and value is {dataArray[i].ToString()}");

                // Simple get of data types from the cache
                string value = db.StringGet(key);
                log.Info($"Database got: {value}");
                }

                return req.CreateResponse(HttpStatusCode.OK, "Got");

            */
        }
    }
}
