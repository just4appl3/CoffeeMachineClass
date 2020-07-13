using CoffeeMachineSimulator.Sender.Model.CoffeeMachine.Simulator.Sender.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeMachineSimulator.Interfaces.Sender
{
    public interface ICoffeMachineDataSender
    {
        Task SendDataAsync(CoffeeMachineData data);

        Task SendDataAsync(IEnumerable<CoffeeMachineData> datas);
    }
}
