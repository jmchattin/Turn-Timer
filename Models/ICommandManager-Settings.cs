﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

using System.Diagnostics;

namespace Turn_Timer_WPF.Models
{
    static class ICommandManager_Settings
    {
        public static ICommand SetupButton()
        {
            Command cmd = new Command( formBtn =>
            {
                System.Windows.Controls.Button btn = ( System.Windows.Controls.Button ) formBtn;
                try
                {
                    var settingsWindow = System.Windows.Application.Current.Windows[1];
                    switch ( btn.Content.ToString() )
                    {
                        case "Save":
                            SaveSettings();
                            break;
                        case "Cancel":
                            settingsWindow.Close();
                            break;
                        default:
                            throw new NotImplementedException( "Unrecognized Button." );
                    }
                }
                catch ( Exception ex )
                {
                    Console.Write( "// TODO : FIX ME", ex );
                    throw;
                }

            }

            );

            return cmd;
        }

        private static void SaveSettings()
        {
            Properties.Settings.Default.roundTime = Convert.ToInt16(ViewModel_Settings.vmInstance.roundTime);
            Properties.Settings.Default.firstPlayerExtraTime = Convert.ToInt16( ViewModel_Settings.vmInstance.extraTime );
            Properties.Settings.Default.timeIsSeconds = ViewModel_Settings.vmInstance.isSeconds;
            Properties.Settings.Default.mutedSound = ViewModel_Settings.vmInstance.muted;
            Properties.Settings.Default.useTimeDiff = ViewModel_Settings.vmInstance.useDiff;

            if ( !Properties.Settings.Default.mutedSound )
                System.Media.SystemSounds.Beep.Play();
        }

    }
}