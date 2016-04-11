using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

using System.Diagnostics;

namespace Turn_Timer_WPF.Models
{
    /// <summary>
    /// Handles all button, checkbox, and other interactions by the user.
    /// </summary>
    static class ICommandManager
    {
        /// <summary>
        /// For each button, determine what commands a given button is to execute.
        /// </summary>
        /// <returns>The appropriate command to execute.</returns>
        public static ICommand SetupButton()
        {
            Command cmd = new Command( formBtn =>
            {
                System.Windows.Controls.Button btn = ( System.Windows.Controls.Button ) formBtn;
                try
                {
                    switch ( btn.Content.ToString() )
                    {
                        case "START":
                            TimerManager.StartTimer();
                            break;
                        case "▍ ▍":
                            TimerManager.PauseTimer( true );
                            break;
                        case "CONT.":
                            TimerManager.PauseTimer( false );
                            break;
                        case "■":
                            TimerManager.StopTimer();
                            break;
                        //case Properties.Settings.Default.beginText:
                        //    TimerManager.GoTimer();
                        //    break;
                        default:
                            // Determine if this is the only special case.
                            if ( btn.Content.ToString() == Properties.Settings.Default.beginText )
                            {
                                TimerManager.GoTimer();
                                break;
                            }
                            throw new NotImplementedException( "Unrecognized Button." );
                    }
                }
                catch ( Exception ex )
                {
                    Console.Write( "// TODO : FIX ME", ex );
                    throw;
                }
            });
            return cmd;
        }

        /// <summary>
        /// Scalable decision-function to determine hyperlink functionality.
        /// </summary>
        /// <returns>The correct command to execute.</returns>
        public static ICommand SetupHyperlink()
        {
            Command cmd = new Command( link =>
            {
                System.Windows.Documents.Hyperlink lnk = ( System.Windows.Documents.Hyperlink ) link;
                try
                {
                    switch ( lnk.Tag.ToString() )
                    {
                        case "Submit Feedback":
                            Process.Start( lnk.NavigateUri.AbsoluteUri );
                            break;
                        default:
                            throw new NotImplementedException( "Unrecognized Hyperlink." );
                    }
                }
                catch ( Exception ex )
                {
                    Console.Write( "// TODO : FIX ME", ex );
                    throw;
                }
            });
            return cmd;
        }

        /// <summary>
        /// Scalable decision-function to determine what text to display where.
        /// </summary>
        /// <returns>The correct text to display.</returns>
        public static ICommand SetupText()
        {
            Command cmd = new Command( formTxt =>
            {
                string fieldText, uri = string.Empty;
                // Determine if the caller is regular text or a link.
                try
                {
                    System.Windows.Controls.TextBlock txt = ( System.Windows.Controls.TextBlock ) formTxt;
                    fieldText = txt.Text;
                }
                catch
                {
                    System.Windows.Documents.Hyperlink lnk = ( System.Windows.Documents.Hyperlink ) formTxt;
                    fieldText = lnk.Tag.ToString();
                    uri = lnk.NavigateUri.AbsoluteUri;
                }

                try
                {
                    switch ( fieldText )
                    {
                        case "Settings":
                            try
                            {
                                var windowExists = System.Windows.Application.Current.Windows[1];
                            }
                            catch
                            {
                                View_Settings settingsView = new View_Settings();
                                settingsView.Show();
                            }
                            break;
                        case "Submit Feedback":
                            Process.Start( uri );
                            break;
                        default:
                            throw new NotImplementedException( "Unrecognized Text or Hyperlinks." );
                    }
                }
                catch ( Exception ex )
                {
                    Console.Write( "// TODO : FIX ME", ex );
                    throw;
                }
            });
            return cmd;
        }
    }
}
