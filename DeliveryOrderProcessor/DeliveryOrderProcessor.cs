using System.Net;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeliveryOrderProcessor;

public class DeliveryOrderProcessor
{
    private readonly ILogger<DeliveryOrderProcessor> _logger;
    private readonly Container _container;

    public DeliveryOrderProcessor(ILogger<DeliveryOrderProcessor> logger)
    {
        _logger = logger;

		var client = new CosmosClient(Environment.GetEnvironmentVariable("AccountEndpoint"), Environment.GetEnvironmentVariable("AccountKey"));

		var database = client.GetDatabase("things");
        var container = database.GetContainer("deliveryorders");

		_container = container;
    }

    [Function("DeliveryOrderProcessor")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var reader = new StreamReader(req.Body);
        var response = await reader.ReadToEndAsync();

        var orderDetailsObj = JsonConvert.DeserializeObject<OrderDetails>(response);

        if (orderDetailsObj == null)
        {
	        return new BadRequestObjectResult("Incoming object is empty.");
        }

        orderDetailsObj.Id = Guid.NewGuid().ToString();

        var dbResponse = await _container.CreateItemAsync<OrderDetails>(orderDetailsObj, PartitionKey.None);

        if (dbResponse.StatusCode == HttpStatusCode.OK)
        {
			return new OkObjectResult("It is done!");
        }

        return new BadRequestResult();
    }
}