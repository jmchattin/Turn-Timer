using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace Turn_Timer_WPF.Models
{
    // TODO : Is Command.cs needed?
    /// <summary>
    /// Wrapper class for bound commands to simplify WPF command binding.
    /// This content came from the MovePlayer WPF project.
    /// </summary>
    class Command : ICommand
    {
        /// <summary>
        /// Action to execute on command.
        /// </summary>
        private Action<object> action;

        /// <summary>
        /// Initializes a new instance of the Command class.
        /// </summary>
        /// <param name="action">Action to execute</param>
        /// <param name="canExecute">Default flag for command being enabled</param>
        public Command( Action<object> action, bool canExecute = true )
        {
            this.action = action;
            this.CanCommandExecute = canExecute;
        }

        /// <summary>
        /// CanExecuteChanged event.
        /// </summary>
        public event EventHandler CanExecuteChanged = new EventHandler( ( o, e ) => { } );

        /// <summary>
        /// Gets or sets a value indicating whether the command can execute.
        /// </summary>
        public bool CanCommandExecute { get; set; }

        /// <summary>
        /// Returns true when command can be executed.
        /// </summary>
        /// <param name="parameter">unused command parameter</param>
        /// <returns>True if the command can execute</returns>
        public bool CanExecute( object parameter )
        {
            return this.CanCommandExecute;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter">handler parameter</param>
        public void Execute( object parameter )
        {
            if ( this.action != null )
            {
                this.action( parameter );
            }
        }
    }
}