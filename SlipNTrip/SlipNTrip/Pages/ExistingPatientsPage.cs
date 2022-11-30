using SlipNTrip.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;

using Xamarin.Forms;

namespace SlipNTrip
{
    public class ExistingPatientsPage : ContentPage
    {
        //private ListView listView;
        private ListView PatientListView;

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "patients.db3");

        Patient patient = new Patient();

        public ExistingPatientsPage()
        {
            this.Title = "Patients";
            var db = new SQLiteConnection(dbPath);

            ToolbarItem helpToolbarItem = new ToolbarItem
            {
                Text = "?",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            helpToolbarItem.Clicked += helpButtonClicked;
            this.ToolbarItems.Add(helpToolbarItem);

            StackLayout stackLayout = new StackLayout();

            SearchBar searchBar = new SearchBar { Placeholder = "Search items..." };
            searchBar.TextChanged += OnTextChanged;
            stackLayout.Children.Add(searchBar);

            var results = db.GetTableInfo("Patient");
            if(results.Count > 0)
            {
                PatientListView = new ListView();
                PatientListView.ItemsSource = db.Table<Patient>().OrderBy(x => x.PatientID).ToList();
                PatientListView.ItemSelected += listView_ItemSelected;
                PatientListView.BackgroundColor = Color.White; // Was needed for Android App
                PatientListView.SeparatorColor = Color.DarkGray; //IDK if needed
                stackLayout.Children.Add(PatientListView);
            }
            
            Content = new ScrollView { Content = stackLayout };
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            patient = (Patient)e.SelectedItem;
            Navigation.PushAsync(new PatientInfoPage(patient));
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);
            SearchBar searchBar = (SearchBar)sender;
            PatientListView.ItemsSource = db.Table<Patient>().Where(x => x.PatientID.Contains(searchBar.Text)).ToList();
        }

        void helpButtonClicked(object sender, EventArgs e)
        {
            string helpMessage = "Purpose: To access patients information by selecting a patients name\n";
            DisplayAlert("Help - View Patients", helpMessage, "Done");
        }
    }
}