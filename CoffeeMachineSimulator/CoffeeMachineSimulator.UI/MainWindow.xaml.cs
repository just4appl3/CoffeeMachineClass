using CoffeeMachineSimulator.Sender;
using CoffeeMachineSimulator.UI.ViewModel;
using System.Windows;

namespace CoffeeMachineSimulator.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeComponent();
            var evConnString = "Endpoint=sb://coffeenamespace.servicebus.windows.net/;SharedAccessKeyName=coffeehub;SharedAccessKey=lnq8b4L/cdiiwQR6yUABa61AZ4YiHJAnifsrtSWLCNs=;EntityPath=coffeehub";
            DataContext = new MainViewModel(new CoffeeMachineDataSender(evConnString));
        }
    }
}
