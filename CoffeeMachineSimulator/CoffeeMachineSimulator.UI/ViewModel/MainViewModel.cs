using CoffeeMachineSimulator.Interfaces.Sender;
using CoffeeMachineSimulator.Sender.Model.CoffeeMachine.Simulator.Sender.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace CoffeeMachineSimulator.UI.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private int _counterCappucino;
        private int _counterEspresso;
        private string _city;
        private string _serialNumber;
        private int _boilerTemp;
        private int _beanLevel;
        private bool _isSendingPeriodically;
        private ICoffeMachineDataSender _dataSender;
        private DispatcherTimer _dispatcherTimer;

        public MainViewModel(ICoffeMachineDataSender dataSender)
        {
            _dataSender = dataSender;
            SerialNumber = Guid.NewGuid().ToString().Substring(0, 8);
            MakeCappuccinoCommand = new DelegateCommand(MakeCappucinno);
            MakeEspressoCommand = new DelegateCommand(MakeEspresso);
            Logs = new ObservableCollection<string>();
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var boilerTempData = CreateCoffeeMachineData(nameof(BoilerTemp), BoilerTemp);
            var beanLevelData = CreateCoffeeMachineData(nameof(BeanLevel), BeanLevel);

            await SendDataAsync(new[] { boilerTempData, beanLevelData });
        }

        public ICommand MakeCappuccinoCommand { get; }

        public ICommand MakeEspressoCommand { get; }

        public ObservableCollection<string> Logs { get; }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                RaisePropertyChanged();
            }
        }

        public string SerialNumber
        {
            get { return _serialNumber; }
            set
            {
                _serialNumber = value;
                RaisePropertyChanged();
            }
        }

        public int CounterCappuccino
        {
            get { return _counterCappucino; }
            set
            {
                _counterCappucino = value;
                RaisePropertyChanged();
            }
        }

        public int CounterEspresso
        {
            get { return _counterEspresso; }
            set
            {
                _counterEspresso = value;
                RaisePropertyChanged();
            }
        }

        public int BoilerTemp
        {
            get { return _boilerTemp; }
            set
            {
                _boilerTemp = value;
                RaisePropertyChanged();
            }
        }

        public int BeanLevel
        {
            get { return _beanLevel; }
            set
            {
                _beanLevel = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSendingPeriodically
        {
            get { return _isSendingPeriodically; }
            set
            {
                if (_isSendingPeriodically != value)
                {
                    _isSendingPeriodically = value;
                    if (_isSendingPeriodically)
                    {
                        _dispatcherTimer.Start();
                    }
                    else
                    {
                        _dispatcherTimer.Stop();
                    }
                }
                RaisePropertyChanged();
            }
        }

        private async void MakeCappucinno()
        {
            CounterCappuccino++;
            var data = CreateCoffeeMachineData(nameof(CounterCappuccino), CounterCappuccino);
            await SendDataAsync(data);
        }

        private async void MakeEspresso()
        {
            CounterEspresso++;
            var data = CreateCoffeeMachineData(nameof(CounterEspresso), CounterEspresso);
            await SendDataAsync(data);
        }

        private CoffeeMachineData CreateCoffeeMachineData(string sensorType, int sensorValue)
        {
            return new CoffeeMachineData
            {
                City = City,
                SerialNumber = SerialNumber,
                SensorType = sensorType,
                SensorValue = sensorValue,
                RecordingTime = DateTime.Now
            };
        }

        private async Task SendDataAsync(CoffeeMachineData coffeeMachineData)
        {
            try
            {
                await _dataSender.SendDataAsync(coffeeMachineData);
                WriteLog($"Send data: {coffeeMachineData.ToString()}");
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }

        }

        private async Task SendDataAsync(IEnumerable<CoffeeMachineData> coffeeMachineDatas)
        {
            try
            {
                await _dataSender.SendDataAsync(coffeeMachineDatas);
                foreach (var data in coffeeMachineDatas)
                {
                    WriteLog($"Send data: {data}");
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }

        private void WriteLog(string logMessage)
        {
            Logs.Insert(0, logMessage);
        }
    }
}
