using Dapr.Actors;
using Dapr.Actors.Runtime;
using Dapr.Client;
using System;

namespace WorkerActorN.Actors
{
    public interface IWorkerActor : IActor

    {
        Task<bool> SetStateWithEtag(string orderId);    
    }

    public class MyData
    {
        public string Property { get; set; }

        public override string ToString()
        {
            var propValue = this.Property == null ? "null" : this.Property;
            return $"Property: {propValue}";
        }
    }
    
    public class WorkerActor : Actor, IWorkerActor
    
    {
        const string STATESTORE = "actorstore";
        private readonly DaprClient _daprClient;
        public WorkerActor(ActorHost host, DaprClient daprClient) : base(host)
    {
        _daprClient = daprClient;
    }

        public async Task<bool> SetStateWithEtag(string orderId)
        {
            Console.WriteLine($"SetStateWithEtag is called with data: {orderId}");

            var (retrievedData, etag) = await _daprClient.GetStateAndETagAsync<MyData>(STATESTORE, orderId);

            Console.WriteLine($"{retrievedData} has an etag: {etag}");

            // ... validate retrievedData and make changes to it.

            if (retrievedData is null)
            {
                retrievedData = new MyData();
            }

            Random random = new Random();
            retrievedData.Property = "Hello " + random.Next(10000).ToString();

            return await _daprClient.TrySaveStateAsync(STATESTORE, orderId, retrievedData, etag); 
        }

        
    }
}