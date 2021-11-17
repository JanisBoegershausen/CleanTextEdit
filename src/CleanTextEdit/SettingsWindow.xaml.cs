using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CleanTextEdit
{
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Reference to the main editor window
        /// </summary>
        private MainWindow mainWindow;

        public SettingsWindow()
        {
            mainWindow = ((MainWindow)Application.Current.MainWindow);
            InitializeComponent();
        }

        private void DragArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Opacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // If the old value is zero, this is beeing set for the first time from settings
            if (e.OldValue != 0)
            {
                Settings.current.opacity = (float)e.NewValue;
                mainWindow.BackgroundRectangle.Opacity = Settings.current.opacity;
            }
        }
    }
}
