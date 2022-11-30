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
    public class PatientInfoPage : ContentPage
    {
        string dbPatientPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "patients.db3");
        string dbTestResultsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testResults.db3");

        private Patient patient;

        private Label patientIDLabel;
        private Label nameLabel;
        private Label genderLabel;
        private Label ageLabel;
        private Label heightLabel;
        private Label weightLabel;
        private Label ShoeSizeLabel;

        private Entry patientIDEntry;
        private Entry nameEntry;
        private Entry genderEntry;
        private Entry ageEntry;
        private Entry heightEntry;
        private Entry weightEntry;
        private Entry shoeSizeEntry;

        private Button updateButton;
        private Button testButton;
        private Button deleteButton;

        public PatientInfoPage(Patient patient)
        {
            AttributeValues attributeValues = new AttributeValues();

            this.patient = patient;
            this.Title = patient.PatientID + ": " + patient.Name;

            ToolbarItem helpToolbarItem = new ToolbarItem
            {
                Text = "?",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            helpToolbarItem.Clicked += helpButtonClicked;
            this.ToolbarItems.Add(helpToolbarItem);

            StackLayout stackLayout = new StackLayout();

            patientIDLabel = new Label
            {
                Text = "Patient ID",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(patientIDLabel);
            patientIDEntry = new Entry
            {
                Placeholder = "M_000",
                Text = patient.PatientID,
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(patientIDEntry);

            nameLabel = new Label
            {
                Text = "Name",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(nameLabel);
            nameEntry = new Entry
            {
                Placeholder = "Jane Doe",
                Text = patient.Name,
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(nameEntry);

            genderLabel = new Label
            {
                Text = "Gender",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(genderLabel);
            genderEntry = new Entry
            {
                Placeholder = "Female",
                Text = patient.Gender,
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(genderEntry);

            ageLabel = new Label
            {
                Text = "Age",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(ageLabel);
            ageEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = "23",
                Text = patient.Age.ToString(),
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(ageEntry);

            heightLabel = new Label
            {
                Text = "Height (ft.in)",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(heightLabel);
            heightEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = "5.5",
                Text = patient.Height.ToString(),
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(heightEntry);

            weightLabel = new Label
            {
                Text = "Weight (lb)",
                FontSize = attributeValues.getLabelFontSize(),
            };
            stackLayout.Children.Add(weightLabel);
            weightEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = "126",
                Text = patient.Weight.ToString(),
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(weightEntry);

            ShoeSizeLabel = new Label
            {
                Text = "Shoe Size",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(ShoeSizeLabel);
            shoeSizeEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = "7",
                Text = patient.ShoeSize.ToString(),
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(shoeSizeEntry);

            updateButton = new Button
            {
                Text = "Update",
                FontSize = attributeValues.getLabelFontSize(),
                BorderWidth = attributeValues.getBorderWidth(),
                BorderColor = Color.DarkGray
            };
            updateButton.Clicked += UpdateButtonClicked;
            stackLayout.Children.Add(updateButton);

            testButton = new Button
            {
                Text = "Test",
                FontSize = attributeValues.getLabelFontSize(),
                BorderWidth = attributeValues.getBorderWidth(),
                BorderColor = Color.DarkGray
            };
            testButton.Clicked += TestButtonClicked;
            stackLayout.Children.Add(testButton);

            deleteButton = new Button
            {
                Text = "Delete",
                FontSize = attributeValues.getLabelFontSize(),
                BorderWidth = attributeValues.getBorderWidth(),
                BorderColor = Color.DarkGray
            };
            deleteButton.Clicked += DeleteButtonClicked;
            stackLayout.Children.Add(deleteButton);

            Content = stackLayout;
        }

        async void UpdateButtonClicked(object sender, EventArgs e)
        {
            this.Title = patientIDEntry.Text + ": " + nameEntry.Text;
            var db = new SQLiteConnection(dbPatientPath);
          
            if (!string.IsNullOrWhiteSpace(patientIDEntry.Text) && !string.IsNullOrWhiteSpace(nameEntry.Text) 
                && !string.IsNullOrWhiteSpace(ageEntry.Text) && !string.IsNullOrWhiteSpace(heightEntry.Text) 
                && !string.IsNullOrWhiteSpace(genderEntry.Text) && !string.IsNullOrWhiteSpace(weightEntry.Text) 
                && !string.IsNullOrWhiteSpace(shoeSizeEntry.Text))
            {
                Patient patient = new Patient()
                {
                    ID = this.patient.ID,
                    PatientID = patientIDEntry.Text,
                    Name = nameEntry.Text,
                    Gender = genderEntry.Text,
                    Age = double.Parse(ageEntry.Text),
                    Height = double.Parse(heightEntry.Text),
                    Weight = double.Parse(weightEntry.Text),
                    ShoeSize = double.Parse(shoeSizeEntry.Text)
                };

                if (!patient.isAgeWithinRange())
                {
                    await DisplayAlert("Patient Information: Error", "Invalid entry for age", "Done");
                }
                else if (!patient.isHeightWithinRange())
                {
                    await DisplayAlert("Patient Information: Error", "Invalid entry for height", "Done");
                }
                else if (!patient.isWeightWithinRange())
                {
                    await DisplayAlert("Patient Information: Error", "Invalid entry for weight", "Done");
                }
                else if (!patient.isShoeSizeWithinRange())
                {
                    await DisplayAlert("Patient Information: Error", "Invalid entry for shoe size", "Done");
                }
                else
                {
                    await DisplayAlert("Patient Infomation: Updated", "Update Successful", "Done");
                    db.Update(patient);
                }
            }

            else
                await DisplayAlert("Patient Infomation: Error", "One or more fields missing information", "Done");
        }

        async void DeleteButtonClicked(object sender, EventArgs e)
        {
            var dbPatient = new SQLiteConnection(dbPatientPath);
            var dbTestResults = new SQLiteConnection(dbTestResultsPath);
            bool response = await DisplayAlert("Patient Information: Delete", "Are you sure you would like to delete the patient?", "Yes", "No");
            if (response)
            {
                dbPatient.Table<Patient>().Delete(x => x.ID == this.patient.ID);
                dbTestResults.Table<TestResults>().Delete(x => x.PatientID == this.patient.ID);
                await Navigation.PushAsync(new ExistingPatientsPage());
            }
        }

        async void TestButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TestPage(patient));
        }

        void helpButtonClicked(object sender, EventArgs e)
        {
            string helpMessage = "Purpose: To view the patients information\n" +
                "Update: Saves the information changed with patients information\n" +
                "Test: Navigates to page to view the list of test results\n" +
                "Delete: Removes patient information and test results from database";
            DisplayAlert("Help - Patient Infomation Page", helpMessage, "Done");
        }
    }
}