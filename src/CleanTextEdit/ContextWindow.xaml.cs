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
        public ContextWindow()
        {
            InitializeComponent();
        }

        private void Save_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                File.WriteAllText(openFileDialog.FileName, ((MainWindow)Application.Current.MainWindow).mainTextField.Text);
            this.Hide();
        }

        private void Load_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                ((MainWindow)Application.Current.MainWindow).mainTextField.Text = File.ReadAllText(openFileDialog.FileName);
            this.Hide();
        }
    }
}
