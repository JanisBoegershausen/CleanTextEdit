using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace CleanTextEdit
{
    /// <summary>
    /// Logic for custom context menu which opens on rightclick
    /// </summary>
    public partial class ContextWindow : Window
    {
        /// <summary>
        /// Reference to the main editor window
        /// </summary>
        private MainWindow mainWindow;

        public ContextWindow()
        {
            mainWindow = ((MainWindow)Application.Current.MainWindow);
            InitializeComponent();
        }

        // ---------------------------------------------
        // --------- Area for button callbacks----------
        // ---------------------------------------------

        private void New_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.New();
            this.Hide();
        }

        private void Save_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.TrySaveCurrent();
            this.Hide();
        }

        private void SaveAs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                mainWindow.SaveAs(saveFileDialog.FileName);
            this.Hide();
        }

        private void Load_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.ShowLoadFileDialog();
            this.Hide();
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.Close();
        }

        private void Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.settingsWindow.Show();
            MainWindow.settingsWindow.Focus();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
