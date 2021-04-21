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
using System.Text.RegularExpressions;

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
            /*
            MessageBox.Show($@"
            You are: {cbiAgab.Content.ToString()}
            Your gender identity: {cbiGender.Content.ToString()}
            Your romantic orientation: {cbiRomantic.Content.ToString()}
            Your sexual orientation: {cbiSexual.Content.ToString()}
            You present: {cbiPresentation.Content.ToString()}
            ");
            */

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
            bool cis = true;

            if (user.IsCis == false || user.PresentsCis == false)
            {
                cis = false;
            }

            if (user.IsHet == false)
            {
                displayHomophobic(countriesAmmended, user, cis);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="substring"></param>
        /// <returns></returns>
        static string deleteSubstring(string content, string substring)
        {
            int index = Convert.ToInt32(content?.IndexOf(substring));
            string output = "";

            for (int i = 0; i < Convert.ToInt32(content?.Length); i++) 
            { 
                if (i == index) { return output; }
                output += content[i]; 
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countriesAmmended"></param>
        /// <returns></returns>
        string displayTransphobic(string[] countriesAmmended)
        {
            string toDisplay = "";
            string modifier = ";";
            for (int i = 0; i < countriesAmmended.Length; i++)
            {
                if (Convert.ToBoolean(countriesAmmended[i]?.Contains("(gender)"))) { toDisplay += "\u2022" + deleteSubstring(countriesAmmended[i]?.ToString(), "(") + modifier; };
                if (Convert.ToBoolean(countriesAmmended[i]?.Contains("(only gender)"))) { toDisplay += "\u2022" + deleteSubstring(countriesAmmended[i].ToString(), "(") + modifier; };
            }


            return toDisplay;
        }

        /// <summary>
        /// Removes any blank elements in the input list
        /// Also merges any instances where two elements were split into multiple due to an awkward split delimeter
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        List<string> sanitiseToDisplay(List<string> content)
        {
            /* DEBUG
            for (int i = 0; i < content.ToString().Length; i++)
            {
                MessageBox.Show(content[i]);
            }
            */

            List<string> newList = new List<string>();

            for (int i = 0; i < content.ToString().Length; i++)
            {
                if (content[i] != " " || content[i] != "")
                {
                    newList.Add(content[i]);
                }
            }

            newList.RemoveAt(0);

            return newList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toDisplay"></param>
        void displayList(string toDisplay)
        {
            List<TextBlock> txtbHolder = new List<TextBlock>(); // All the text blocks, add a new Add call here if a new block is added
            txtbHolder.Add(txtbCountries);
            txtbHolder.Add(txtbCountries2);

            //string[,] txtHolder = new string[txtbHolder.Count, 24];
            List<string> txtHolder = new List<string>();

            List<string> toDisplayList = sanitiseToDisplay(toDisplay.Split('\u2022').ToList<string>()); // The list of countries

            int counter = 0; // The 0 -> 24 counter for how many countries can fit in one text block
            int currentChunk = 0; // The txtBlock being written to currently

            for (int i = 0; i < toDisplayList.Count; i++)
            {
                //txtbHolder[currentChunk].Text += toDisplayList[i];
                txtHolder.Add(toDisplayList[i].ToString());

                if (counter > 24) { currentChunk++; counter = 0; }

                else { counter++; }
            }

            //throw new Exception(); // DEBUG

            int boxCounter = 0;
            int displayCounter = 0;

            try
            {
                /*
                for (int i = 0; i < txtHolder.Count; i++)
                {
                    txtbHolder[i].Text = "\u2022" + txtHolder[i] + "\n";
                }
                */
                for (int i = 0; i < txtHolder.Count; i++)
                {
                    if (displayCounter == 24)
                    {
                        boxCounter++;
                        displayCounter = 0;
                    }

                    txtbHolder[boxCounter].Text += "\u2022" + txtHolder[i] + "\n";

                    displayCounter++;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countriesAmmended"></param>
        /// <param name="user"></param>
        void displayHomophobic(string[] countriesAmmended, User user, bool cis)
        {
            string toDisplay = "";
            string modifier = "; ";
            for (int i = 0; i < countriesAmmended.Length; i++)
            {
                if (!(Convert.ToBoolean(countriesAmmended[i]?.Contains("(only gender)")))) { toDisplay += "\u2022" + deleteSubstring(countriesAmmended[i]?.ToString(), "(") + modifier; };
            }

            if (cis == false)
            {
                toDisplay += displayTransphobic(countriesAmmended);
            }

            displayList(deleteEmpty(toDisplay));

            //txtbCountries.Text = deleteEmpty(toDisplay);
            //MessageBox.Show(deleteEmpty(toDisplay)); // DEBUG
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string deleteEmpty(string content)
        {
            string output = "";

            // Put content into an array based on the delimeter ";"
            string[] contentArr = content.Split(';');

            // Enumerate through the array, checking each element to see if it doesn't contain [a-q] (lower it all first)
            string pattern = @"[a-q]";
            Regex rx = new Regex(pattern);
            
            for (int i = 0; i < contentArr.Length; i++)
            {
                if (rx.IsMatch(contentArr[i].ToLower()))
                {
                    output += contentArr[i];
                }
            }

            return output;
        }
    }
}
// Each textblock (atm) can hold 24 countries vertically