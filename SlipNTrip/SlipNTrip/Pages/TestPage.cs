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
    public class TestPage : ContentPage
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testResults.db3");

        private Patient patient;
        private TestResults testResults = new TestResults();
        private ListView testResultsListView;
        
        public TestPage(Patient patient)
        {
            var db = new SQLiteConnection(dbPath);

            this.patient = patient;
            this.Title = "Test";

            ToolbarItem newTestToolbarItem = new ToolbarItem
            {
                Text = "+\t",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            newTestToolbarItem.Clicked += newTestButtonClicked;
            this.ToolbarItems.Add(newTestToolbarItem);

            ToolbarItem homeToolbarItem = new ToolbarItem
            {
                Text = "Home\t",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };
            homeToolbarItem.Clicked += homeToolbarItemClicked;
            this.ToolbarItems.Add(homeToolbarItem);

            ToolbarItem helpToolbarItem = new ToolbarItem
            {
                Text = "?",
                Order = ToolbarItemOrder.Primary,
                Priority = 2
            };
            helpToolbarItem.Clicked += helpButtonClicked;
            this.ToolbarItems.Add(helpToolbarItem);

            StackLayout stackLayout = new StackLayout();

            SearchBar searchBar = new SearchBar { Placeholder = "Search items..." };
            searchBar.TextChanged += OnTextChanged;
            stackLayout.Children.Add(searchBar);

            var tableInfo = db.GetTableInfo("TestResults");
            if(tableInfo.Count > 0)
            {
                testResultsListView = new ListView();
                testResultsListView.ItemsSource = db.Table<TestResults>().Where(x => x.PatientID == patient.ID).ToList();
                testResultsListView.ItemSelected += listView_ItemSelected;
                testResultsListView.SeparatorColor = Color.DarkGray;    //IDK if needed
                stackLayout.Children.Add(testResultsListView);
            }
            Content = stackLayout;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);
            SearchBar searchBar = (SearchBar)sender;
            testResultsListView.ItemsSource = db.Table<TestResults>().Where(x => x.TestName.Contains(searchBar.Text)).ToList();
        }

        async void newTestButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeviceControlsPage(patient));
        }

        async void homeToolbarItemClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            testResults = (TestResults)e.SelectedItem;
            Navigation.PushAsync(new Pages.TestResultPage(patient, testResults, true));
        }

        void helpButtonClicked(object sender, EventArgs e)
        {
            string helpMessage = "Purpose: To access patients test results by selecting a test name\n" +
                "New Test: Navigates to device controls page to input required information needed for device movements" +
                "and generate the pertubation"; //Check spelling
            DisplayAlert("Help - View Test Results", helpMessage, "Done");
        }
    }
}