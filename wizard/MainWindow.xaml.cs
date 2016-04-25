using System;
using System.Windows;

namespace wizard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // button events handlers:

        private void button_Click(object sender, RoutedEventArgs e) // opens wizard window
        {
            try
            {
                var win = new WizardWindow();
                win.ShowDialog();
                if (win.FilledBoxes < 8)  // Checks if all required fields in Wizard were filled
                    MessageBox.Show("Process isn't completed! Launch application again.");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Error occured (Invalid Operation Exception): {0}", ex.Message);
            }
        }

        private void button_Click_1(object sender, RoutedEventArgs e) // closes main window
        {
            Close();
        }
    }
}
