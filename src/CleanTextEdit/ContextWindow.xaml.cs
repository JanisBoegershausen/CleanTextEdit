using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                mainWindow.SaveAs(openFileDialog.FileName);
            this.Hide();
        }

        private void Load_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainWindow.ShowLoadFileDialog();
            this.Hide();
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            mainWindow.Close();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
