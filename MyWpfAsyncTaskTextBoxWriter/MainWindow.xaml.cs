using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyWpfAsyncTaskTextBoxWriter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Task _task;
        CancellationTokenSource _ts;
        CancellationToken _ct;

        #region INotify Changed Properties  
        private string output;
        public string Output
        {
            get { return output; }
            set { SetField(ref output, value, nameof(Output)); }
        }

        // Template for a new INotify Changed Property
        // for using CTRL-R-R
        private bool xxx;
        public bool Xxx
        {
            get { return xxx; }
            set { SetField(ref xxx, value, nameof(Xxx)); }
        }
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            EnableDisableButtons(false);
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_Start_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            StartMyWriteToTexBoxTask();
            EnableDisableButtons(true);
        }
        private void StartMyWriteToTexBoxTask()
        {
            _ts = new CancellationTokenSource();
            _ct = _ts.Token;
            _task = Task.Factory.StartNew(() => WriteToTextBox(), _ct);
        }
        private void WriteToTextBox()
        {
            int c = 0;

            // Do it for ever until main thread requested to cancel
            while(true)
            {
                Output += String.Format("{0} ", ++c);

                if (_ct.IsCancellationRequested)
                {
                    // UI thread decided to cancel
                    Output += String.Format("We have a Cancellation Requested ");
                    break;
                }

                // Do some thing
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Button_Cancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ts.Cancel();
            EnableDisableButtons(false);
        }

        /// <summary>
        /// Button_Clear_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            Output = "";
        }

        /// <summary>
        /// Button_Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// EnableDisableButtons
        /// </summary>
        /// <param name="running"></param>
        private void EnableDisableButtons(bool running)
        {
            button_Start_Task.IsEnabled = !running;
            button_Cancel_Task.IsEnabled = running;
        }

        /// <summary>
        /// SetField
        /// for INotify Changed Properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
