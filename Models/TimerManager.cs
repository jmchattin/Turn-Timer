﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Threading;

using System.Media;

using System.Threading;

namespace Turn_Timer_WPF.Models
{
    /// <summary>
    /// Handles all things timer-related.
    /// </summary>
    class TimerManager
    {
        private static DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Normal);

        // TODO : Hit-up adding a debug feature to the program.

        // Timer values.
        private static TimeSpan overflow;
        private static TimeSpan resetTime;
        private static TimeSpan roundTime;
        private static DateTime startTime;
        private static DateTime pauseTime;

        // C:\Windows\Media
            // Windows User Account Control.wav
            // Windows Exclamation.wav
            // Windows Error.wav
            // Windows Hardware Fail.wav
            // Windows Unlock.wav
            // Windows Critical Stop.wav
            // Windows Battery Critical.wav
            // Player sounds and other values.
        private static SoundPlayer player = new SoundPlayer( @"C:\Windows\Media\Windows User Account Control.wav" );
        private static bool isPlaying = false;
        private static Thread playerThread; 


        /// <summary>
        /// Instantialize the timer with default values.
        /// </summary>
        public TimerManager()
        {
            _timer.Interval = TimeSpan.FromMilliseconds( 1 );
            _timer.Tick += timer_Tick;

            player.LoadAsync();
        }

        /// <summary>
        /// Get new timer values, finish initialization, and change input controls.
        /// </summary>
        public static void StartTimer()
        {
            // Get new values for the timer.
            overflow = new TimeSpan( 0, 0, Properties.Settings.Default.firstPlayerExtraTime );
            int intRoundTime = ( int ) Properties.Settings.Default.roundTime;
            resetTime = ( Properties.Settings.Default.timeIsSeconds ) ?
                new TimeSpan( 0, 0, intRoundTime ) : new TimeSpan( 0, intRoundTime, ( int ) ( Properties.Settings.Default.roundTime % intRoundTime * 60 ) );

            // Start the timer.
            roundTime = ( resetTime + overflow );
            startTime = DateTime.Now;

            _timer.Start();
            //Console.Write( "check _timer" );
            // Change input control values.
            ViewModel.vmInstance.startGoBtn = Properties.Settings.Default.beginText;
            ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );

            ViewModel.vmInstance.startedBtnVis = System.Windows.Visibility.Visible.ToString();
            ViewModel.vmInstance.OnPropertyChanged( "startedBtnVis" );
        }

        /// <summary>
        /// Temporarily stop the timer, change input controls.
        /// </summary>
        /// <param name="pause">Pause or unpaused bool value.</param>
        public static void PauseTimer( bool pause = false )
        {
            if ( pause )
            {
                pauseTime = DateTime.Now;
                // Stop the timer.
                _timer.IsEnabled = false;
                ViewModel.vmInstance.startGoBtn = "CONT.";
                ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );
            }
            else
            {
                startTime += DateTime.Now - pauseTime;
                // Restart the timer.
                _timer.IsEnabled = true;
                ViewModel.vmInstance.startGoBtn = Properties.Settings.Default.beginText;
                ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );
            }
            Console.Write("check _timer");
        }

        /// <summary>
        /// Completely stop the timer, reset input controls, stop any sounds.
        /// </summary>
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

        /// <summary>
        /// Collect how much time has passed on the timer, reset timer to new starting time.
        /// </summary>
        public static void GoTimer()
        {
            TimeSpan checkTime = roundTime - ( DateTime.Now - startTime );

            overflow = new TimeSpan( 0, 0, 0 );
            
            if ( Properties.Settings.Default.useTimeDiff &&
                checkTime.Seconds > 0 && checkTime < resetTime )
                overflow = resetTime - checkTime;

            // Increase the time that the timer now has to count down.
            roundTime = resetTime + overflow;
            startTime = DateTime.Now;

            if ( !_timer.IsEnabled )
            {
                _timer.Start();

                ViewModel.vmInstance.startGoBtn = Properties.Settings.Default.beginText;
                ViewModel.vmInstance.OnPropertyChanged( "startGoBtn" );

                ViewModel.vmInstance.stopVis = System.Windows.Visibility.Hidden.ToString();
                ViewModel.vmInstance.OnPropertyChanged( "stopVis" );
            }

            ViewModel.vmInstance.stopLbl = "";
            ViewModel.vmInstance.OnPropertyChanged( "stopLbl" );
            ViewModel.vmInstance.stopVis = System.Windows.Visibility.Hidden.ToString();
            ViewModel.vmInstance.OnPropertyChanged( "stopVis" );
            player.Stop();
        }

        /// <summary>
        /// Count down the remaining time.  Beep if time has run out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick( object sender, EventArgs e )
        {
            TimeSpan passedTime = DateTime.Now - startTime;

            if ( passedTime <= roundTime )
            {
                TimeSpan nTime = ( roundTime - passedTime );
                ViewModel.vmInstance.turnTime = string.Format( "{0:00}:{1:00}.{2:000}",
                    nTime.Minutes,
                    nTime.Seconds,
                    nTime.Milliseconds );
                ViewModel.vmInstance.OnPropertyChanged( "turnTime" );
            }
            else
            {
                // There is no more time.
                // TODO : Add better beeping!  Add Flashing!

                ViewModel.vmInstance.turnTime = "00:00.000";
                ViewModel.vmInstance.OnPropertyChanged( "turnTime" );

                ViewModel.vmInstance.stopLbl = "!STOP! The turn is over. !STOP!";
                ViewModel.vmInstance.OnPropertyChanged( "stopLbl" );

                ViewModel.vmInstance.stopVis = System.Windows.Visibility.Visible.ToString();
                ViewModel.vmInstance.OnPropertyChanged( "stopVis" );

                if ( !isPlaying && !Properties.Settings.Default.mutedSound )
                {
                    // Play a beeping sound.
                    playerThread = new Thread( playStopThread );
                    playerThread.Start();
                }
            }
        }

        /// <summary>
        /// Play a looping sound until told to stop.
        /// </summary>
        private void playStopThread()
        {
            isPlaying = true;
            player.PlaySync();
            isPlaying = false;
        }
    }
}
