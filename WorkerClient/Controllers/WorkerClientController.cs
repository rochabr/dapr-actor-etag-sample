
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using WorkerClient.Models;
using WorkerActorN.Actors;
using Dapr.Actors;
using Dapr.Actors.Client;

namespace WorkerClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerClientController : ControllerBase
    {
        private readonly ILogger<WorkerClientController> _logger;

         private const string OrderTopic = "orders";
        private const string PubSubName = "actorpubsub";

        private const string WorkerActorType = "WorkerActor";

        private readonly DaprClient _daprClient;

        public WorkerClientController(ILogger<WorkerClientController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }
        
        [Dapr.Topic(PubSubName, OrderTopic)]
        [HttpPost("/orders")]
        public async Task<IActionResult> Process([FromBody] OrderSummary orderSummary)
        {   
            if (orderSummary is not null) 
            {
                _logger.LogInformation(
                "Starting order completion for {orderId}.",
                orderSummary.OrderId);

                var oId = orderSummary.OrderId.ToString();
                var actorId = new ActorId(oId);

                var proxy = ActorProxy.Create<IWorkerActor>(actorId, WorkerActorType);
                await proxy.SetStateWithEtag(oId);

                var result = await proxy.GetDataAsync();
                Console.WriteLine($"Data from actor: {result}");

                return Ok();
            
            }
            
            return BadRequest();
        }

    }
}