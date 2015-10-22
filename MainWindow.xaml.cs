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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Runtime.Remoting.Messaging;


namespace AsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            btnDownload.IsEnabled = false;
            //DownloadSynchronous();
            //DownloadEap(); 
            //DownloadApm(); //Begin End
            DownloadTpl2();
            //DownloadAsyncAwait();
        }

        #region synchronous
        private void DownloadSynchronous()
        {
            Thread.Sleep(5000);
            txtContent.Text = "Downloaded Synchronously!";
            btnDownload.IsEnabled = true;
        }
        #endregion

        #region APM
        private void DownloadApm()
        {
            BeginGetDownloadApmMessage(ApmCallback, Guid.NewGuid());
        }

        private void ApmCallback(IAsyncResult result)
        {           
            string message = $"{((AsyncState)result.AsyncState).State} - {EndGetDownloadApmMessage(result)}";
            Dispatcher.Invoke(() =>
            {
                txtContent.Text = message;
                btnDownload.IsEnabled = true;
            });

        }

        private class AsyncState
        {
            public object State;
            public object Func;
        }

        private IAsyncResult BeginGetDownloadApmMessage(AsyncCallback callback, object state)
        {
            Func<string> func = GetDownloadApmMessage;
            return func.BeginInvoke(callback, new AsyncState() { Func = func, State = state });
        }

        private string EndGetDownloadApmMessage(IAsyncResult result)
        {            
            var func = (Func<string>)((AsyncResult)result).AsyncDelegate;
            return func.EndInvoke(result);
        }

        private string GetDownloadApmMessage()
        {
            Thread.Sleep(5000);
            return "Downloaded using APM";
        }
        #endregion

        #region EAP

        private void DownloadEap()
        {
            txtContent.Text = "Just kidding, I got bored looking at how to implement EAP.";
        }

        #endregion

        #region TPL
        private void DownloadTpl()
        {
            Task.Run(() =>
            {
                Thread.Sleep(5000);
                return "Downloaded using TPL";

            }).ContinueWith( (t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    btnDownload.IsEnabled = true;
                    txtContent.Text = t.Result;
                });
            });
        }

        private void DownloadTpl2()
        {
            var task = Task.Run(() =>
            {
                Thread.Sleep(5000);
                return "Downloaded using tpl2";
            });

            task.ConfigureAwait(true).GetAwaiter().OnCompleted(() =>
            {
                btnDownload.IsEnabled = true;
                txtContent.Text = task.Result;
            });
        }
        #endregion

        #region AsyncAwait
        private async void DownloadAsyncAwait()
        {
            var message = await Task.Run(() =>
            {
                Thread.Sleep(2000);
                return "Downloaded using AsyncAwait";
            });
            btnDownload.IsEnabled = true;
            txtContent.Text = message;

        }
        #endregion  


    }
}
