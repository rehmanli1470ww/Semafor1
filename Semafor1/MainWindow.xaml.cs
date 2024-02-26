using EntityFremworkImtahan.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Semafor1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, NotificationService
    {
        private ObservableCollection<Thread> watingthreads;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public ObservableCollection<Thread> Workingthreads {  get; set; }
        public ObservableCollection<Thread> Createthreads {  get; set; }
        public ObservableCollection<Thread> Watingthreads { get => watingthreads; set { watingthreads = value; OnPropertyChanged(); } }
        public Semaphore semaphore { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
           
            Workingthreads = new ObservableCollection<Thread>();
            Createthreads= new ObservableCollection<Thread>();
            Watingthreads= new ObservableCollection<Thread>();
            semaphore= new Semaphore(1,1,"Semafor");
        }

        public void Create(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() => {

            });
            thread.Name = "Thread" + thread.ManagedThreadId;
            Createthreads.Add(thread);
        }
        public void Waiting_Thread(object sender, RoutedEventArgs e)
        {
            var IntialCount=int.Parse(numberTextBox.Text);
            var ListItem = (ListView)sender;
            var temp = (Thread)ListItem.SelectedItem;
            semaphore = new Semaphore(IntialCount, IntialCount, "Semafor");
            new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Watingthreads.Add(temp);
                    Createthreads.Remove(temp);

                }));


                semaphore.WaitOne(2000);
                Thread.Sleep(1000);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Workingthreads.Add(temp);
                    Watingthreads.Remove(temp);

                }));


                semaphore.Release();
               
            }).Start();
            
        }

       

    }
}