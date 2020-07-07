using CoffeeMachineSimulator.Sender.Model.CoffeeMachine.Simulator.Sender.Model;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachineSimulator.Sender
{
    public interface ICoffeMachineDataSender
    {
        Task SendDataAsync(CoffeeMachineData data);
        Task SendDataAsync(IEnumerable<CoffeeMachineData> datas);
    }

    public class CoffeeMachineDataSender : ICoffeMachineDataSender
    {
        private EventHubClient _ehClient;

        public CoffeeMachineDataSender(string ehConnString)
        {
            _ehClient = EventHubClient.CreateFromConnectionString(ehConnString);

        }

        public async Task SendDataAsync(CoffeeMachineData data)
        {
            EventData evData = CreateEventData(data);
            var partitions = await _ehClient.GetRuntimeInformationAsync();
            var partitionId = await _ehClient.GetPartitionRuntimeInformationAsync(partitions.PartitionIds.First());

            await _ehClient.SendAsync(evData);
        }

        public async Task SendDataAsync(IEnumerable<CoffeeMachineData> datas)
        {
            var eventDatas = datas.Select(x => CreateEventData(x));
            var evDataBatch = _ehClient.CreateBatch();


            foreach (var evData in eventDatas)
            {
                if (!evDataBatch.TryAdd(evData))
                {
                    await _ehClient.SendAsync(evDataBatch);
                    evDataBatch = _ehClient.CreateBatch();
                    evDataBatch.TryAdd(evData);
                }
            }
            if (evDataBatch.Count > 0)
            {
                await _ehClient.SendAsync(evDataBatch);
            }
        }

        private static EventData CreateEventData(CoffeeMachineData data)
        {
            var dataAsJson = JsonConvert.SerializeObject(data);
            var evData = new EventData(Encoding.UTF8.GetBytes(dataAsJson));
            return evData;
        }
    }
}
