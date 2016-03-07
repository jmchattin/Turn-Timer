using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel; // INotify

using System.Windows.Input; // ICommand

namespace Turn_Timer_WPF
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region VARIABLES
        public event PropertyChangedEventHandler PropertyChanged;

        private static ViewModel _vmInstance;
        public static ViewModel vmInstance { get { return _vmInstance; } set { _vmInstance = value; } }

        private ICommand _buttonCmd;
        public ICommand buttonCmd { get{ return _buttonCmd; } set{ _buttonCmd = value; } }

        private ICommand _textCmd;
        public ICommand textCmd { get { return _textCmd; } set { _textCmd = value; } }

        private string _turnTime;
        public string turnTime { get { return _turnTime; } set { _turnTime = value; } }

        private string _startGoBtn = "START";
        public string startGoBtn { get { return _startGoBtn; } set { _startGoBtn = value; } }

        private string _startedBtnVis = System.Windows.Visibility.Hidden.ToString();
        public string startedBtnVis { get { return _startedBtnVis; } set { _startedBtnVis = value; } }

        private string _stopVis = System.Windows.Visibility.Hidden.ToString();
        public string stopVis { get { return _stopVis; } set { _stopVis = value; } }

        // TODO : Add random list of fun STOP verbage, such as "!STOP and get gud!", "TOO SLOW!"
        private string _stopLbl = "!STOP!";
        public string stopLbl { get { return _stopLbl; } set { _stopLbl = value; } }

        #endregion // VARIABLES

        public ViewModel()
        {
            vmInstance = this;

            Models.TimerManager timerManager = new Models.TimerManager();

            this.buttonCmd = Models.ICommandManager.SetupButton();

            this.textCmd = Models.ICommandManager.SetupText();
        }

        // TODO : Set-up a window-close method to close the settings window if the main window is closed.

        public void OnPropertyChanged( string name )
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( name ) );
            }
        }
    }
}
