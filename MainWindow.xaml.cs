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

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> identity =
                new Dictionary<string, string>(); // identity.Add();

            ComboBoxItem cbiAgab = (ComboBoxItem)comboAgab.SelectedItem;
            ComboBoxItem cbiGender = (ComboBoxItem)comboGender.SelectedItem;
            ComboBoxItem cbiRomantic = (ComboBoxItem)comboRomantic.SelectedItem;
            ComboBoxItem cbiSexual = (ComboBoxItem)comboSexual.SelectedItem;
            ComboBoxItem cbiPresentation = (ComboBoxItem)comboPresentation.SelectedItem;

            identity.Add("agab", cbiAgab.Content.ToString()); //TODO: FIx blank input bug causing crash
            identity.Add("gender", cbiGender.Content.ToString());
            identity.Add("romantic", cbiRomantic.Content.ToString());
            identity.Add("sexual", cbiSexual.Content.ToString());
            identity.Add("presentation", cbiPresentation.Content.ToString());


            MessageBox.Show($@"
            You are: {cbiAgab.Content.ToString()}
            Your gender identity: {cbiGender.Content.ToString()}
            Your romantic orientation: {cbiRomantic.Content.ToString()}
            Your sexual orientation: {cbiSexual.Content.ToString()}
            You present: {cbiPresentation.Content.ToString()}
            ");

            Dictionary<string, string> hate = new Dictionary<string, string>();

            StreamReader sr = new StreamReader("C:\\Users\\marfx\\source\\repos\\LgbtqBannedPlacesCsWpf\\hate.json");

            string json = sr.ReadToEnd();

            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                switch (reader.Value)
                {
                    case null:
                        break;

                    default:
                        if (Convert.ToString(reader.TokenType) != "StartObject")
                        {
                            hate.Add()
                        }

                        break;
                }
                /*
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
        }
    }
}
