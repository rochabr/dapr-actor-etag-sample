﻿// using Dapr.Actors;
// using Dapr.Actors.Runtime;

// namespace WorkerClient.Models
// {
// public interface IWorkerActor : IActor
// {
//         Task<string> SetDataAsync(MyData data);
//         Task<MyData> GetDataAsync();
//         Task RegisterReminder();
//         Task UnregisterReminder();
//         Task<IActorReminder> GetReminder();
//         Task RegisterTimer();
//         Task UnregisterTimer();
//     }

//     public class MyData
//     {
//         public string PropertyA { get; set; }
//         public string PropertyB { get; set; }

//         public override string ToString()
//         {
//             var propAValue = this.PropertyA == null ? "null" : this.PropertyA;
//             var propBValue = this.PropertyB == null ? "null" : this.PropertyB;
//             return $"PropertyA: {propAValue}, PropertyB: {propBValue}";
//         }
//     }
// }
