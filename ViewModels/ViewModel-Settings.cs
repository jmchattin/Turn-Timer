using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel; // INotify

using System.Windows.Input; // ICommand

namespace Turn_Timer_WPF
{
    /// <summary>
    /// Base class for the Settings popup window.
    /// </summary>
    class ViewModel_Settings : INotifyPropertyChanged
    {
        #region VARIABLES
        public event PropertyChangedEventHandler PropertyChanged;

        public static ViewModel_Settings vmInstance;

        private ICommand _buttonCmd;
        public ICommand buttonCmd { get { return _buttonCmd; } set { _buttonCmd = value; } }

        private int _roundTime = Properties.Settings.Default.roundTime;
        public string roundTime
        {
            get { return _roundTime.ToString(); }
            set {
                int result;
                if ( int.TryParse( value, out result ) && result > 0 )
                    _roundTime = result; }
        }

        private int _extraTime = Properties.Settings.Default.firstPlayerExtraTime;
        public string extraTime
        {
            get { return _extraTime.ToString(); }
            set
            {
                int result;
                if ( int.TryParse( value, out result ) && result >= 0 )
                    _extraTime = result;
            }
        }

        private bool _isSeconds = Properties.Settings.Default.timeIsSeconds;
        public bool isSeconds { get { return _isSeconds; } set { _isSeconds = value; } }

        private bool _muted = Properties.Settings.Default.mutedSound;
        public bool muted { get { return _muted; } set { _muted = value; } }

        private bool _useDiff = Properties.Settings.Default.useTimeDiff;
        public bool useDiff { get { return _useDiff; } set { _useDiff = value; } }

        #endregion // VARIABLES

        /// <summary>
        /// Initialization of callable instance and control setup.
        /// </summary>
        public ViewModel_Settings()
        {
            vmInstance = this;

            this.buttonCmd = Models.ICommandManager_Settings.SetupButton();
            //this.chkbxCmd = Models.ICommandManager_Settings.SetupCheckbox();
        }

        /// <summary>
        /// Update the content of an object.
        /// </summary>
        /// <param name="name">The object to update.</param>
        public void OnPropertyChanged( string name )
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( name ) );
            }
        }
    }
}
