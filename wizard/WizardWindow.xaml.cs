using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace wizard
{
    /// <summary>
    /// Interaction logic for WizardWindow.xaml
    /// </summary>
    
    public partial class WizardWindow : Window
    {
        SqlConnection conn = new SqlConnection
            (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=
             Users.mdf;Integrated Security=True");
        SqlCommand com = new SqlCommand();
        SqlDataReader dReader;

        public WizardWindow()
        {
            InitializeComponent();      
        }

        // Event handlers for changing of text in text boxes:

        private void firstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(firstNameTextBox.Text))
            {
                Page1.CanSelectNextPage = false;
            }
            else
            {
                Page1.CanSelectNextPage = true;
            }
            completionProgressChecker();
            progressBarFilled.Value = progressValue;
        }

        private void lastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(lastNameTextBox.Text))
            {
                Page2.CanSelectNextPage = false;
            }
            else
            {
                Page2.CanSelectNextPage = true;
                nameTextBlock.Text = firstNameTextBox.Text +
                    " " + 
                    lastNameTextBox.Text;
            }
            completionProgressChecker();
            progressBarFilled2.Value = progressValue;
        }

        private void streetTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(streetTextBox.Text))
            {
                houseTextBox.IsEnabled = false;
            }
            else
            {
                houseTextBox.IsEnabled = true;
            }
            addressCheckingHelper();
            completionProgressChecker();
            progressBarFilled3.Value = progressValue;
        }

        private void houseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(houseTextBox.Text))
            {
                flatTextBox.IsEnabled = false;
                postalTextBox.IsEnabled = false;
            }
            else
            {
                flatTextBox.IsEnabled = true;
                postalTextBox.IsEnabled = true;
            }
            addressCheckingHelper();
            completionProgressChecker();
            progressBarFilled3.Value = progressValue;
        }

        private void flatTextBox_TextChanged(object sender, TextChangedEventArgs e) // optional box
        {
            if (string.IsNullOrEmpty(flatTextBox.Text))
            {
                streetTextBlock.Text = streetTextBox.Text + 
                    " " +
                    houseTextBox.Text;
            }
            else
            {
                streetTextBlock.Text = streetTextBox.Text + 
                    " " +
                    houseTextBox.Text +
                    "/" +
                    flatTextBox.Text;
            }
        }

        private void postalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(postalTextBox.Text))
            {
                cityTextBox.IsEnabled = false;
            }
            else
            {
                cityTextBox.IsEnabled = true;
            }
            addressCheckingHelper();
            completionProgressChecker();
            progressBarFilled3.Value = progressValue;
        }

        private void cityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(cityTextBox.Text))
            {
                countryTextBox.IsEnabled = false;
            }
            else
            {
                countryTextBox.IsEnabled = true;
                postalAndCityTextBlock.Text = postalTextBox.Text + 
                    " " + 
                    cityTextBox.Text;
            }
            addressCheckingHelper();
            completionProgressChecker();
            progressBarFilled3.Value = progressValue;
        }

        private void countryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(countryTextBox.Text))
            {
            }
            else
            {
                countryTextBlock.Text = countryTextBox.Text;
            }
            addressCheckingHelper();
            completionProgressChecker();
            progressBarFilled3.Value = progressValue;
        }

        private void phoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number;
            bool result = int.TryParse(phoneTextBox.Text, out number);

            if (string.IsNullOrEmpty(phoneTextBox.Text))
            {
                Page4.CanSelectNextPage = false;
            }
            else if (result)
            {
                Page4.CanSelectNextPage = true;
                phoneTextBlock.Text = phoneTextBox.Text;
            }
            else
            {
                MessageBox.Show("You have to type phone number in that field!");
                Page4.CanSelectNextPage = false;
            }
            completionProgressChecker();
            progressBarFilled4.Value = progressValue;
        }

        // Event handlers for KeyDown (Enter key) action in Address boxes:

        private void streetTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                houseTextBox.Focus();
        }

        private void houseTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                flatTextBox.Focus();
        }

        private void flatTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                postalTextBox.Focus();
        }

        private void postalTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cityTextBox.Focus();
        }

        private void cityTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                countryTextBox.Focus();
        }

        // Event handlers for page's loading:
        // - focus inside of box/first box on the page
        // - Progress Bar status

        private void Page1_Loaded(object sender, RoutedEventArgs e)
        {
            firstNameTextBox.Focus();
            completionProgressChecker();
            progressBarFilled.Value = progressValue;
        }

        private void Page2_Loaded(object sender, RoutedEventArgs e)
        {
            lastNameTextBox.Focus();
            completionProgressChecker();
            progressBarFilled2.Value = progressValue;
        }

        private void Page3_Loaded(object sender, RoutedEventArgs e)
        {
            streetTextBox.Focus();
            completionProgressChecker();
            progressBarFilled3.Value = progressValue;
        }

        private void Page4_Loaded(object sender, RoutedEventArgs e)
        {
            phoneTextBox.Focus();
            completionProgressChecker();
            progressBarFilled4.Value = progressValue;
        }

        private void Page5_Loaded(object sender, RoutedEventArgs e)
        {
            Page5.CanSelectNextPage = false;
            com.Connection = conn;
        }

        //Event handler: "Save data" button

        private void InsertData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                com.CommandText = "select max (userid) from users";
                dReader = com.ExecuteReader();
                dReader.Read();
                string maxId = "";
                maxId = dReader[0].ToString();
                conn.Close();
                int newId = int.Parse(maxId) + 1;
                conn.Open();
                com.CommandText =
                    "insert into users (Userid, Name, StreetAndHouseNumber, PostalCode, City, Country, Phone) values ('" + newId +
                    "','" + nameTextBlock.Text + "','" + streetTextBlock.Text + "','" + postalTextBox.Text + "','" +
                    cityTextBox.Text + "','" + countryTextBox.Text + "','" + phoneTextBox.Text + "')";
                com.ExecuteNonQuery();
                com.Clone();
                conn.Close();
                MessageBox.Show("Data saved");

                Page5.CanSelectNextPage = true;
            }
            catch (SqlException)
            {
                MessageBox.Show("There's a problem with SQL! Probably directory name is wrong.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something is wrong: {0}", ex.Message);
            }
        }

        // Helper for checking if all address fields are filled (page 3):

        private void addressCheckingHelper()
        {
            if ((string.IsNullOrEmpty(streetTextBox.Text)) ||
                (string.IsNullOrEmpty(houseTextBox.Text)) ||
                (string.IsNullOrEmpty(postalTextBox.Text)) ||
                (string.IsNullOrEmpty(cityTextBox.Text)) ||
                (string.IsNullOrEmpty(countryTextBox.Text)))
            {
                Page3.CanSelectNextPage = false;
            }
            else
            {
                Page3.CanSelectNextPage = true;
            }
        }
        
        // checks how many fields are filled:

        private double progressValue; // Value for progress bar

        public int FilledBoxes; // overall number of filled required boxes

        private void completionProgressChecker()
        {
            int c1 = (string.IsNullOrEmpty(firstNameTextBox.Text)) ? 0 : 1;
            int c2 = (string.IsNullOrEmpty(lastNameTextBox.Text)) ? 0 : 1;
            int c3 = (string.IsNullOrEmpty(streetTextBox.Text)) ? 0 : 1;
            int c4 = (string.IsNullOrEmpty(houseTextBox.Text)) ? 0 : 1;
            int c5 = (string.IsNullOrEmpty(postalTextBox.Text)) ? 0 : 1;
            int c6 = (string.IsNullOrEmpty(cityTextBox.Text)) ? 0 : 1;
            int c7 = (string.IsNullOrEmpty(countryTextBox.Text)) ? 0 : 1;
            int c8 = (string.IsNullOrEmpty(phoneTextBox.Text)) ? 0 : 1;

            FilledBoxes = (c1 + c2 + c3 + c4 + c5 + c6 + c7 + c8);

            switch (FilledBoxes)
            {
                case 0:
                    progressValue = 0;
                    break;
                case 1:
                    progressValue = 12.5;
                    break;
                case 2:
                    progressValue = 25;
                    break;
                case 3:
                    progressValue = 37.5;
                    break;
                case 4:
                    progressValue = 50;
                    break;
                case 5:
                    progressValue = 62.5;
                    break;
                case 6:
                    progressValue = 75;
                    break;
                case 7:
                    progressValue = 87.5;
                    break;
                case 8:
                    progressValue = 100;
                    break;
            }
        }
    }

}
