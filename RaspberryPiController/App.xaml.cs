using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RaspberryPiController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Internal error: " + e.Exception.Message +
                            ". You can continue to use the application, but it may not work properly.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}