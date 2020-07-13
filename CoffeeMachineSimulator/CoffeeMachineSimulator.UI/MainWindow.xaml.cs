using CoffeeMachineSimulator.Implementation.Sender;
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
            var evConnString = "Endpoint=sb://coffemachineeventhubns.servicebus.windows.net/;SharedAccessKeyName=coffemachinepolicy;SharedAccessKey=z/YG/0fEEgVte1nj/2dzwhKw/weEtXRcBZp3RBBlNvg=;EntityPath=coffeemachineeventhub";
            DataContext = new MainViewModel(new CoffeeMachineDataSender(evConnString));
        }
    }
}
