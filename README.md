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

## Running the sample

Initialize Dapr locally:

```bash
dapr init
```

Update the `pubsub.yaml` file with your Azure Service Bus Topic information and run both apps:

**Worker Actor**

```bash
dapr run --app-id worker-actor --app-port 7100 --resources-path ../resources --log-level debug -- dotnet run
```

**Worker Client**

```bash
apr run --app-id worker-client --app-protocol http --app-port 5700 --dapr-http-port 5780 --dapr-grpc-port 5701 --resources-path ../resources -- dotnet run
```

**Publish event to topic**

Send the following payload to the topic `orders`:

```json
{
  "topic": "orders",
  "pubsubname": "actorpubsub",
  "traceid": "00-113ad9c4e42b27583ae98ba698d54255-e3743e35ff56f219-01",
  "tracestate": "",
  "data": {
    "orderId": "1"
  },
  "id": "5929aaac-a5e2-4ca1-859c-edfe73f11565",
  "specversion": "1.0",
  "datacontenttype": "application/json; charset=utf-8",
  "source": "checkout",
  "type": "com.dapr.event.sent",
  "time": "2020-09-23T06:23:21Z",
  "traceparent": "00-113ad9c4e42b27583ae98ba698d54255-e3743e35ff56f219-01"
}
```
