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
    /// Interaktionslogik für ContextWindow.xaml
    /// </summary>
    public partial class ContextWindow : Window
    {
        /// <summary>
        /// Path to the current file that is beieng worked on (if any)
        /// </summary>
        private string currentWorkingPath = "";

        public ContextWindow()
        {
            InitializeComponent();
        }

        private void Save_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!String.IsNullOrEmpty(currentWorkingPath) && File.Exists(currentWorkingPath))
                SaveAs(currentWorkingPath);
            this.Hide();
        }

        private void SaveAs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                SaveAs(openFileDialog.FileName);
            this.Hide();
        }

        private void Load_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                Load(openFileDialog.FileName);
            this.Hide();
        }

        void SaveAs(string path)
        {
            File.WriteAllText(path, ((MainWindow)Application.Current.MainWindow).mainTextField.Text);
            currentWorkingPath = path;
        }

        void Load(string path)
        {
            currentWorkingPath = path;
            ((MainWindow)Application.Current.MainWindow).mainTextField.Text = File.ReadAllText(path);
        }
    }
}
