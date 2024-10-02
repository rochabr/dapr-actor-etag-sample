# Dapr Actor etag sample

## WorkerActor

WorkerActor represents the Actor to be instantiated.

To use optimistic concurrency control (OCC) with a first-write-wins strategy, `SetStateWithEtag` retrieves the current ETag using the `DaprClient.GetStateAndETagAsync` method. Then writes the updated value and passes along the retrieved ETag using the `DaprClient.TrySaveStateAsync` method.

The `DaprClient.TrySaveStateAsync` method fails when the data (and associated ETag) has been changed in the state store after the data was retrieved. The method returns a boolean value to indicate whether the call succeeded. A strategy to handle the failure is to simply reload the updated data from the state store, make the change again, and resubmit the update.

```csharp
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
```

## WorkerClient

WorkerClient is Web API with a route that is triggered by a Dapr subscription pointing to a topic in resources/pubsub.yaml.

To test:

