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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CleanTextEdit
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Hotkey Commands
        public static RoutedCommand SaveCommand = new RoutedCommand();

        /// <summary>
        /// The context window which opens on rightclick
        /// </summary>
        static ContextWindow contextWindow;

        /// <summary>
        /// Path to the current file that is beieng worked on (if any)
        /// </summary>
        private string currentWorkingPath = "";

        public MainWindow()
        {
            // Initialize
            InitializeComponent();

            // Try loading the settings from the settings file
            if (!Settings.TryLoadFromFile(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "cte.ini")))
                Settings.current = new Settings(); // Create new default settings

            InitializeHotkeys();

            // Create an instance of the contextMenu to use every time the user right clicks
            contextWindow = new ContextWindow();

            // Try opening the last opened file
            if (!TryLoad(Settings.current.startupPath))
                mainTextField.Text = "Hello there! \nWelcome to the cleanest text editor in town. \nUse right-click to access the menu. ";
        }

        private void InitializeHotkeys()
        {
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        }

        // -----------------------------------------
        // ----------- Hotkey Callbacks ------------
        // -----------------------------------------

        private void SaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TrySaveCurrent();
        }

        // -----------------------------------------
        // ------------ Input Callbacks ------------
        // -----------------------------------------

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void TextBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            contextWindow.Left = this.PointToScreen(Mouse.GetPosition(this)).X;
            contextWindow.Top = this.PointToScreen(Mouse.GetPosition(this)).Y;
            contextWindow.Show();
            contextWindow.Focus();
        }

        // -----------------------------------------
        // ----------- Editor Utilities ------------
        // -----------------------------------------

        /// <summary>
        /// Saves to the currentWorkingPath, if it is set.
        /// </summary>
        public void TrySaveCurrent()
        {
            if (!String.IsNullOrEmpty(currentWorkingPath) && File.Exists(currentWorkingPath))
                SaveAs(currentWorkingPath);
        }

        public void SaveAs(string path)
        {
            File.WriteAllText(path, mainTextField.Text);
            currentWorkingPath = path;
        }

        public bool TryLoad(string path)
        {
            if (!String.IsNullOrEmpty(path) && File.Exists(path))
            {
                currentWorkingPath = path;
                mainTextField.Text = File.ReadAllText(path);
                return true;
            } else
            {
                return false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.current.startupPath = currentWorkingPath;
            Settings.SaveCurrent(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "cte.ini"));
        }
    }
}
