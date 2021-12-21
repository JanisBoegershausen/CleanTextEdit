using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CleanTextEdit
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Hotkey Commands
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand OpenCommand = new RoutedCommand();
        public static RoutedCommand NewCommand = new RoutedCommand();
        public static RoutedCommand ZoomInCommand = new RoutedCommand();
        public static RoutedCommand ZoomOutCommand = new RoutedCommand();

        /// <summary>
        /// The context window which opens on rightclick
        /// </summary>
        static ContextWindow contextWindow;
        public static SettingsWindow settingsWindow;

        /// <summary>
        /// Path to the current file that is beieng worked on (if any)
        /// </summary>
        private string currentWorkingPath = "";
        /// <summary>
        /// If true, the user is warned about unsaved changed on the next attempt to close the application. 
        /// </summary>
        private bool unsavedChangesWarning = false;

        /// <summary>
        /// Point in time where the last autosave occured. 
        /// </summary>
        private DateTime lastSaveTime;

        public MainWindow()
        {
            // Initialize
            InitializeComponent();

            // Try loading the settings from the settings file
            if (!Settings.TryLoadFromFile(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "cte.ini")))
                Settings.current = new Settings(); // Create new default settings

            // Add an event listener to use as a main loop
            CompositionTarget.Rendering += OnCompositionTargetRendering;

            InitializeHotkeys();

            // Create an instance of the contextMenu to use every time the user right clicks and same for the settings window
            contextWindow = new ContextWindow();
            settingsWindow = new SettingsWindow();

            // Apply loaded settings
            BackgroundRectangle.Opacity = Settings.current.opacity;

            // Set the option sliders to match the loaded settigns
            settingsWindow.Slider_Opacity.Value = Settings.current.opacity;
            settingsWindow.Checkbox_Autosave.IsChecked = Settings.current.autosave;
            settingsWindow.Checkbox_AlwaysOnTop.IsChecked = Settings.current.alwaysOnTop;

            // Try opening the last opened file
            WriteToLog("Trying to load startup file... ");
            if (!TryLoad(Settings.current.startupPath))
                mainTextField.Text = "Hello there! \nWelcome to the cleanest text editor in town. \nUse right-click to access the menu. ";

            WriteToLog("Startup Complete.");
        }

        /// <summary>
        /// Called every frame before the application renders. 
        /// </summary>
        void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            // Handle autosaving every 60 seconds if it is enabled
            if(Settings.current.autosave && (DateTime.Now - lastSaveTime).TotalSeconds >= 60f)
            {
                // Only try to save if there is a currentWorkingPath. This is to avoid spamming the log
                if (!String.IsNullOrEmpty(currentWorkingPath) && File.Exists(currentWorkingPath))
                {
                    WriteToLog("Trying to autosave...");
                    TrySaveCurrent();
                }
            }

            // Update window settings
            Topmost = Settings.current.alwaysOnTop && (!MainWindow.settingsWindow.IsVisible);

            // Fade all log entries to become transparent over time
            for (int i = 0; i < LogContainer.Children.Count; i++)
            {
                LogContainer.Children[i].Opacity -= 0.003f; // Warning: This is framerate dependant!
            }
        }

        private void InitializeHotkeys()
        {
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            NewCommand.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            ZoomInCommand.InputGestures.Add(new KeyGesture(Key.Add, ModifierKeys.Control));
            ZoomOutCommand.InputGestures.Add(new KeyGesture(Key.Subtract, ModifierKeys.Control));
        }

        // -----------------------------------------
        // ----------- Hotkey Callbacks ------------
        // -----------------------------------------

        private void SaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TrySaveCurrent();
        }

        private void OpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ShowLoadFileDialog();
        }

        private void NewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            New();
        }

        private void ZoomInCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomIn();
        }

        private void ZoomOutCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomOut();
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

        private void mainTextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            unsavedChangesWarning = true;
        }

        // -----------------------------------------
        // ----------- Editor Utilities ------------
        // -----------------------------------------

        /// <summary>
        /// Starts a new document, by clearing the current working directory and the text field
        /// </summary>
        public void New()
        {
            currentWorkingPath = "";
            mainTextField.Text = "";
        }


        /// <summary>
        /// Saves to the currentWorkingPath, if it is set.
        /// </summary>
        public void TrySaveCurrent()
        {
            if (!String.IsNullOrEmpty(currentWorkingPath) && File.Exists(currentWorkingPath))
            {
                SaveAs(currentWorkingPath);
            }
            else
            {
                WriteToLog("Can't save! No file is opened.");
            }
        }

        /// <summary>
        /// Save the written text to a file at the given path. 
        /// </summary>
        public void SaveAs(string path)
        {
            // Save the content of the editor to the given path
            File.WriteAllText(path, mainTextField.Text);
            currentWorkingPath = path;
            unsavedChangesWarning = false;

            // Output user message to log
            WriteToLog("Saved!");

            // Set lastSaveTime to now
            lastSaveTime = DateTime.Now;
        }

        /// <summary>
        /// Try loading the content of a file at the given path into the editor. 
        /// </summary>
        public bool TryLoad(string path)
        {
            if (!String.IsNullOrEmpty(path) && File.Exists(path))
            {
                currentWorkingPath = path;
                mainTextField.Text = File.ReadAllText(path);
                unsavedChangesWarning = false;

                WriteToLog("File has been opened.");

                // Set lastSaveTime to now
                lastSaveTime = DateTime.Now;

                return true;
            } else
            {
                WriteToLog("Failed to load file!");
                return false;
            }
        }

        /// <summary>
        /// Increase the font size of the edited text.
        /// </summary>
        public void ZoomIn()
        {
            mainTextField.FontSize += 1;
        }

        /// <summary>
        /// Decrease the font size of the edited text.
        /// </summary>
        public void ZoomOut()
        {
            mainTextField.FontSize -= 1;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (unsavedChangesWarning)
            {
                e.Cancel = true;
                WriteToLog("Unsaved changed! Are you sure?");
                unsavedChangesWarning = false;
            }
            else
            {
                contextWindow.Close();
                settingsWindow.Close();
                Settings.current.startupPath = currentWorkingPath;
                Settings.SaveCurrent(Path.Combine(Directory.GetCurrentDirectory(), "cte.ini"));
            }
        }
    
        /// <summary>
        /// Writes the given string to the editors log, located at the bottom right. 
        /// </summary>
        public void WriteToLog(string message)
        {
            // Create the label
            Label messageLabel = new Label();
            messageLabel.Content = message;
            messageLabel.Foreground = Brushes.White;
            
            // Add it to the log container as a child
            LogContainer.Children.Add(messageLabel);

            // Limit the log count to three
            if (LogContainer.Children.Count > 3)
            {
                LogContainer.Children.Remove(LogContainer.Children[0]);
            }
        }

        public void ShowLoadFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                TryLoad(openFileDialog.FileName);
        }
    }
}
