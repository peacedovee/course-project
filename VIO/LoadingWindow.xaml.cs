using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using VIO;

namespace VIO
{
    /// <summary>
    /// Логика взаимодействия для LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(LoadingWindow_Loaded);
        }

        private void LoadingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Thread loadingThread = new Thread(new ThreadStart(SimulateLoading));
            loadingThread.Start();
        }

        private void SimulateLoading()
        {
            Dispatcher.Invoke(() => progressBar.Value = 0);
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50); // Имитация загрузки
                Dispatcher.Invoke(() =>
                {
                    progressBar.Value = i;
                    progressText.Text = $"{i}%";
                });
            }
            Dispatcher.Invoke(() =>
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            });
        }
    }
}