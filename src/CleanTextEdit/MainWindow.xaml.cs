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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CleanTextEdit
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static ContextWindow contextWindow;

        public MainWindow()
        {
            InitializeComponent();
            contextWindow = new ContextWindow();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void TextBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            contextWindow.Left = Application.Current.MainWindow.PointToScreen(Mouse.GetPosition(Application.Current.MainWindow)).X;
            contextWindow.Top = Application.Current.MainWindow.PointToScreen(Mouse.GetPosition(Application.Current.MainWindow)).Y;
            contextWindow.Show();
            contextWindow.Focus();
        }
    }
}
