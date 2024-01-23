using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cloudie.Commands
{
    /// <summary>
    /// Base class for commands.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// Invokes the CanExecuteChanged event.
        /// </summary>
        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged.Invoke(this, new EventArgs());
        }
    }
}
