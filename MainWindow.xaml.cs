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
using Newtonsoft.Json;
using System.IO;

namespace LgbtqBannedPlacesCsWpf
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

        /// <summary>
        /// When the button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Creates the dictionary for the user's identity
            Dictionary<string, string> identity =
                new Dictionary<string, string>(); // identity.Add();

            // Creates objects for each combobox, when the user adds their identity
            ComboBoxItem cbiAgab = (ComboBoxItem)comboAgab.SelectedItem;
            ComboBoxItem cbiGender = (ComboBoxItem)comboGender.SelectedItem;
            ComboBoxItem cbiRomantic = (ComboBoxItem)comboRomantic.SelectedItem;
            ComboBoxItem cbiSexual = (ComboBoxItem)comboSexual.SelectedItem;
            ComboBoxItem cbiPresentation = (ComboBoxItem)comboPresentation.SelectedItem;

            // Turns all of those objects into strings
            identity.Add("agab", cbiAgab.Content.ToString()); //TODO: FIx blank input bug causing crash
            identity.Add("gender", cbiGender.Content.ToString());
            identity.Add("romantic", cbiRomantic.Content.ToString());
            identity.Add("sexual", cbiSexual.Content.ToString());
            identity.Add("presentation", cbiPresentation.Content.ToString());

            // Spits out the identity to the user (debug)
            MessageBox.Show($@"
            You are: {cbiAgab.Content.ToString()}
            Your gender identity: {cbiGender.Content.ToString()}
            Your romantic orientation: {cbiRomantic.Content.ToString()}
            Your sexual orientation: {cbiSexual.Content.ToString()}
            You present: {cbiPresentation.Content.ToString()}
            ");

            // Creates a dictionary for the JSON to sit in
            Dictionary<string, string> hate = new Dictionary<string, string>();

            // Creates a streamreader object for reading hateCountries.json
            StreamReader sr = new StreamReader("C:\\Projects\\LbtqBannedPlacesCsWpf2\\LgbtqBannedPlacesCsWpf\\hateCountries.json");

            // Reads hateCountries.json and puts it in json (the string, not the format)
            string json = sr.ReadToEnd();

            // Creates a list for all the countries with anti-lgbtq+ laws
            List<string> countries = new List<string>();

            // Reads the JSON values
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            // Operates as a counter (I hate it too, don't worry)
            int i = 0;
            // While there is more to be read
            while (reader.Read())
            { 
                switch (reader.Value)
                {
                    // If the value is null then stop
                    case null:
                        break;

                    // Else, if it isn't startobject (Not useful to me rn) then add it to the list
                    default:
                        if (Convert.ToString(reader.TokenType) != "StartObject")
                        {
                            //MessageBox.Show($"{reader.TokenType}, {reader.Value}"); // Country, empty string
                            countries.Add(reader.Value.ToString()); // Adding to the list
                            
                            // Increment the counter
                            ++i;
                        }

                        break;
                }
                /* (FROM DOCS)
                if (reader.Value != null)
                {
                    MessageBox.Show($"Token: {reader.TokenType}, Value: {reader.Value}");
                }
                else
                {
                    MessageBox.Show($"Token: {reader.TokenType}");
                }
                */
            }
            
            // An array to store the countries without the blank strings
            string[] countriesAmmended = new string[countries.Count];
            int counter = 0;

            // For every element in countries
            for (int j = 0; j < countries.Count; ++j)
            {
                // If it isn't whitespace
                if (countries[j] != "")
                { 
                    // Add it to the array
                    countriesAmmended.SetValue(countries[j], ++counter);
                }
            }

            bool isCis = true;
            bool presentsCis = true;
            bool isHet = true;

            // If the user isn't cis
            if (identity["agab"] != identity["gender"]) { isCis = false; }

            // If the user is assigned GNC
            if (identity["agab"] != identity["presentation"]) { presentsCis = false; }

            // If the user isn't heterosexual
            if (identity["sexual"] != "Heterosexual") { isHet = false; }

            // If the user isn't heteromantic
            if (identity["romantic"] != "Heteromantic") { isHet = false; }


            User user = new User(isCis, isHet, presentsCis);

            displayCountries(user, countriesAmmended);
        }

        /// <summary>
        /// A method which calls the transphobic and homophobic countries to be called
        /// </summary>
        /// <param name="user"></param>
        void displayCountries(User user, string[] countriesAmmended)
        {
            if (user.IsCis == false || user.PresentsCis == false)
            {
                displayTransphobic(countriesAmmended);
            }

            if (user.IsHet == false)
            {
                displayHomophobic();
            }
        }

        static string deleteSubstring(string content, string substring)
        {
            int index = content.IndexOf(substring);
            string output = "";

            for (int i = 0; i < content.Length; i++) 
            { 
                if (i == index) { return output; }
                output += content[i]; 
            }

            return output;
        }

        void displayTransphobic(string[] countriesAmmended)
        {
            string toDisplay = "";
            for (int i = 0; i < countriesAmmended.Length; i++)
            {
                if (Convert.ToBoolean(countriesAmmended[i]?.Contains("(gender)"))) { toDisplay += deleteSubstring(countriesAmmended[i], "(") + "\n"; };
                if (Convert.ToBoolean(countriesAmmended[i]?.Contains("(only gender)"))) { toDisplay += deleteSubstring(countriesAmmended[i], "(") + "\n"; };
            }

            txtbCountries.Text = toDisplay;
        }

        static void displayHomophobic()
        {

        }
    }
}
