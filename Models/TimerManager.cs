using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Threading;

using System.Media;

using System.Threading;

namespace Turn_Timer_WPF.Models
{
    class TimerManager
    {
        private static DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Normal);

        // TODO : Hit-up adding a debug feature to the program.

        private static TimeSpan roundTime;
        private static TimeSpan overflow;
        private static TimeSpan resetTime;

        // C:\Windows\Media
            // Windows User Account Control.wav
            // Windows Exclamation.wav
            // Windows Error.wav
            // Windows Hardware Fail.wav
            // Windows Unlock.wav
            // Windows Critical Stop.wav
            // Windows Battery Critical.wav
        private static SoundPlayer player = new SoundPlayer( @"C:\Windows\Media\Windows User Account Control.wav" );
        private static bool isPlaying = false;
        private static Thread playerThread; 

        public TimerManager()
        {
            _timer.Interval = TimeSpan.FromMilliseconds( 1 );
            _timer.Tick += timer_Tick;

            player.LoadAsync();
        }

        public static void StartTimer()
        {
            overflow = new TimeSpan( 0, 0, Properties.Settings.Default.firstPlayerExtraTime );
            resetTime = ( Properties.Settings.Default.timeIsSeconds ) ?
                new TimeSpan( 0, 0, Properties.Settings.Default.roundTime ) : new TimeSpan( 0, Properties.Settings.Default.roundTime, 0 );

            roundTime = resetTime + overflow;
            _timer.Start();
            Console.Write( "check _timer" );
            ViewModel.vmInstance.startGoBtn = "GO";
            ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );

            ViewModel.vmInstance.startedBtnVis = System.Windows.Visibility.Visible.ToString();
            ViewModel.vmInstance.OnPropertyChanged( "startedBtnVis" );
        }

        public static void PauseTimer(bool pause = false)
        {
            if ( pause )
            {
                _timer.IsEnabled = false;
                ViewModel.vmInstance.startGoBtn = "CONT.";
                ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );
            }
            else
            {
                _timer.IsEnabled = true;
                ViewModel.vmInstance.startGoBtn = "GO";
                ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );
            }

            Console.Write("check _timer");
        }

        public static void StopTimer()
        {
            _timer.Stop();
            Console.Write( "check _timer" );
            ViewModel.vmInstance.startGoBtn = "START";
            ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );

            ViewModel.vmInstance.startedBtnVis = System.Windows.Visibility.Hidden.ToString();
            ViewModel.vmInstance.OnPropertyChanged( "startedBtnVis" );

            ViewModel.vmInstance.stopVis = System.Windows.Visibility.Hidden.ToString();
            ViewModel.vmInstance.OnPropertyChanged( "stopVis" );

            player.Stop();
        }

        public static void GoTimer()
        {
            overflow = new TimeSpan( 0, 0, 0 );
            // && (resetTime - roundTime).TotalSeconds > 0
            if ( Properties.Settings.Default.useTimeDiff && roundTime.TotalSeconds > 0 && roundTime < resetTime )
                overflow = resetTime - roundTime;

            roundTime = resetTime + overflow;

            if ( !_timer.IsEnabled )
            {
                _timer.Start();

                ViewModel.vmInstance.startGoBtn = "GO";
                ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );

                ViewModel.vmInstance.stopVis = System.Windows.Visibility.Hidden.ToString();
                ViewModel.vmInstance.OnPropertyChanged( "stopVis" );
            }

            player.Stop();
        }

        private void timer_Tick( object sender, EventArgs e )
        {
            if ( roundTime.TotalMilliseconds > 0 )
            {
                roundTime = roundTime.Add( TimeSpan.FromMilliseconds( -10 ) );

                ViewModel.vmInstance.turnTime = string.Format("{0:00}:{1:00}.{2:000}", roundTime.Minutes, roundTime.Seconds, roundTime.Milliseconds);
                ViewModel.vmInstance.OnPropertyChanged( "turnTime" );
            }
            else
            {
                // TODO : Add beeping!  Add Flashing!

                ViewModel.vmInstance.turnTime = "00:00.000";
                ViewModel.vmInstance.OnPropertyChanged( "turnTime" );

                ViewModel.vmInstance.stopLbl = "!STOP! The turn is over. !STOP!";
                ViewModel.vmInstance.OnPropertyChanged( "stopLbl" );

                ViewModel.vmInstance.stopVis = System.Windows.Visibility.Visible.ToString();
                ViewModel.vmInstance.OnPropertyChanged( "stopVis" );

                if ( !isPlaying && !Properties.Settings.Default.mutedSound )
                {
                    playerThread = new Thread( playStopThread );
                    playerThread.Start();
                }
            }
        }

        private void playStopThread()
        {
            isPlaying = true;
            player.PlaySync();
            isPlaying = false;
        }
    }
}
