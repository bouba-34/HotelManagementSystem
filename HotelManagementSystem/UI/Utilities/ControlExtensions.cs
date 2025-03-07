// HotelManagementSystem.UI/Utilities/ControlExtensions.cs
using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace HotelManagementSystem.UI.Utilities
{
    /// <summary>
    /// Extension methods for Windows Forms controls to help with asynchronous operations
    /// and other common UI tasks.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Executes the specified action on the UI thread of the control.
        /// </summary>
        /// <param name="control">The control on which to execute the action.</param>
        /// <param name="action">The action to execute.</param>
        public static void InvokeIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
        
        /// <summary>
        /// Executes the specified function on the UI thread of the control and returns the result.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="control">The control on which to execute the function.</param>
        /// <param name="func">The function to execute.</param>
        /// <returns>The result of the function.</returns>
        public static T InvokeIfRequired<T>(this Control control, Func<T> func)
        {
            if (control.InvokeRequired)
            {
                return (T)control.Invoke(func);
            }
            else
            {
                return func();
            }
        }
        
        /// <summary>
        /// Executes an async operation with UI feedback (wait cursor, disabled controls).
        /// </summary>
        /// <param name="control">The control that initiates the operation.</param>
        /// <param name="asyncOperation">The async operation to execute.</param>
        /// <param name="showWaitCursor">Whether to show a wait cursor during the operation.</param>
        /// <param name="disableControl">Whether to disable the control during the operation.</param>
        /// <returns>A task representing the async operation.</returns>
        public static async Task ExecuteWithUIFeedbackAsync(this Control control, 
            Func<Task> asyncOperation, 
            bool showWaitCursor = true, 
            bool disableControl = true)
        {
            var previousCursor = control.Cursor;
            var wasEnabled = control.Enabled;
            
            try
            {
                if (showWaitCursor)
                {
                    control.InvokeIfRequired(() => control.Cursor = Cursors.WaitCursor);
                }
                
                if (disableControl)
                {
                    control.InvokeIfRequired(() => control.Enabled = false);
                }
                
                await asyncOperation();
            }
            finally
            {
                control.InvokeIfRequired(() =>
                {
                    if (showWaitCursor)
                    {
                        control.Cursor = previousCursor;
                    }
                    
                    if (disableControl)
                    {
                        control.Enabled = wasEnabled;
                    }
                });
            }
        }
        
        /// <summary>
        /// Executes an async operation with UI feedback (wait cursor, disabled controls) and returns a result.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <param name="control">The control that initiates the operation.</param>
        /// <param name="asyncOperation">The async operation to execute.</param>
        /// <param name="showWaitCursor">Whether to show a wait cursor during the operation.</param>
        /// <param name="disableControl">Whether to disable the control during the operation.</param>
        /// <returns>A task representing the async operation with the result.</returns>
        public static async Task<T> ExecuteWithUIFeedbackAsync<T>(this Control control, 
            Func<Task<T>> asyncOperation, 
            bool showWaitCursor = true, 
            bool disableControl = true)
        {
            var previousCursor = control.Cursor;
            var wasEnabled = control.Enabled;
            
            try
            {
                if (showWaitCursor)
                {
                    control.InvokeIfRequired(() => control.Cursor = Cursors.WaitCursor);
                }
                
                if (disableControl)
                {
                    control.InvokeIfRequired(() => control.Enabled = false);
                }
                
                return await asyncOperation();
            }
            finally
            {
                control.InvokeIfRequired(() =>
                {
                    if (showWaitCursor)
                    {
                        control.Cursor = previousCursor;
                    }
                    
                    if (disableControl)
                    {
                        control.Enabled = wasEnabled;
                    }
                });
            }
        }
        
        /// <summary>
        /// Sets the enabled state of multiple controls at once.
        /// </summary>
        /// <param name="enabled">The enabled state to set.</param>
        /// <param name="controls">The controls to update.</param>
        public static void SetControlsEnabled(bool enabled, params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.InvokeIfRequired(() => control.Enabled = enabled);
            }
        }
        
        /// <summary>
        /// Shows or hides multiple controls at once.
        /// </summary>
        /// <param name="visible">The visibility state to set.</param>
        /// <param name="controls">The controls to update.</param>
        public static void SetControlsVisible(bool visible, params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.InvokeIfRequired(() => control.Visible = visible);
            }
        }
        
        /// <summary>
        /// Centers the control in its parent container.
        /// </summary>
        /// <param name="control">The control to center.</param>
        public static void CenterInParent(this Control control)
        {
            if (control.Parent == null)
                return;
                
            control.Left = (control.Parent.ClientSize.Width - control.Width) / 2;
            control.Top = (control.Parent.ClientSize.Height - control.Height) / 2;
        }
        
        /// <summary>
        /// Shows a message dialog with an error icon and OK button.
        /// </summary>
        /// <param name="control">The control to use as owner for the dialog.</param>
        /// <param name="message">The error message to display.</param>
        /// <param name="title">The title of the dialog.</param>
        public static void ShowError(this Control control, string message, string title = "Error")
        {
            control.InvokeIfRequired(() => 
                MessageBox.Show(control, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error));
        }
        
        /// <summary>
        /// Shows a message dialog with an information icon and OK button.
        /// </summary>
        /// <param name="control">The control to use as owner for the dialog.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The title of the dialog.</param>
        public static void ShowInfo(this Control control, string message, string title = "Information")
        {
            control.InvokeIfRequired(() => 
                MessageBox.Show(control, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information));
        }
        
        /// <summary>
        /// Shows a confirmation dialog with Yes/No buttons.
        /// </summary>
        /// <param name="control">The control to use as owner for the dialog.</param>
        /// <param name="message">The confirmation message to display.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <returns>True if the user clicked Yes, false otherwise.</returns>
        public static bool Confirm(this Control control, string message, string title = "Confirm")
        {
            return control.InvokeIfRequired(() => 
                MessageBox.Show(control, message, title, MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes);
        }
    }
}