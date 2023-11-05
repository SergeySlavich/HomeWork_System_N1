using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HomeWork_System_N1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //3. Реализовать приложение согласно заданию:
        //Написать десктопную утилиту, выводящую список запущенных процессов.
        //В списке выводится имя процесса и его PID.
        //При клике на элемент списка в доп окне открывается более детальная информация о данном процессе.
        //Список процессов обновляется по клику либо по таймеру.
        //За основу взять пример со стр. 26 урока№ 1.

        private DispatcherTimer timer;
        Process[] processes;

        public MainWindow()
        {
            InitializeComponent();
            LoadProcessInfo();
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 10);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            LoadProcessInfo();
        }

        private void LoadProcessInfo()
        {
            ProcessList.Items.Clear();
            processes = Process.GetProcesses();
            ProcessList.Items.Add($"Count of processes: {processes.Length}");
            ProcessList.Items.Add("PID - name");
            foreach (Process process in processes)
            {
                string pid, name;
                try
                {
                    pid = process.Id.ToString();
                }
                catch
                {
                    pid = "unavailable";
                }
                try
                {
                    name = process.ProcessName.ToString();
                }
                catch
                {
                    name = "unavailable";
                }
                ProcessList.Items.Add($"{pid} - {name}");
            }
        }

        private void ProcessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Process selectedProc = null;
            foreach(Process proc in processes)
            {
                if ($"{proc.Id.ToString()} - {proc.ProcessName}" == ProcessList.SelectedItem.ToString()) selectedProc = proc;
            }
            MessageBox.Show($"Процесс с Id: {selectedProc?.Id} называется \"{selectedProc?.ProcessName}\", имеет приоритет {selectedProc?.BasePriority}");
        }

        private void manualUpdate_Click(object sender, RoutedEventArgs e)
        {
            LoadProcessInfo();
        }

        private void AutoUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                AutoUpdate.Content = "Включить автообновление";
                AutoUpdate.Background = Brushes.LightGreen;
                timer.Stop();
            }
            else
            {
                AutoUpdate.Content = "Выключить автообновление";
                AutoUpdate.Background = Brushes.Gray;
                timer.Start();
            }
        }
    }
}
