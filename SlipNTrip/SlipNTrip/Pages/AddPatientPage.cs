using SlipNTrip.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xamarin.Forms;
using SQLite;

namespace SlipNTrip
{
    public class AddPatientPage : ContentPage
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "patients.db3");
        
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

        private Button saveButton;

        public AddPatientPage()
        {
            AttributeValues attributeValues = new AttributeValues();

            this.Title = "Add Patient";

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
                FontSize = attributeValues.getLabelFontSize()
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
                Placeholder = "24",
                FontSize = attributeValues.getLabelFontSize()
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
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(heightEntry);

            weightLabel = new Label
            {
                Text = "Weight (lb)",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(weightLabel);
            weightEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = "130",
                FontSize = attributeValues.getLabelFontSize()
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
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(shoeSizeEntry);

            saveButton = new Button
            {
                Text = "Save",
                FontSize = attributeValues.getLabelFontSize(),
                BorderWidth = attributeValues.getBorderWidth(),
                BorderColor = Color.DarkGray
            };
            saveButton.Clicked += OnButtonClicked;
            stackLayout.Children.Add(saveButton);

            Content = stackLayout;
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Patient>();

            if (!string.IsNullOrWhiteSpace(patientIDEntry.Text) && !string.IsNullOrWhiteSpace(nameEntry.Text)
                && !string.IsNullOrWhiteSpace(ageEntry.Text) && !string.IsNullOrWhiteSpace(heightEntry.Text)
                && !string.IsNullOrWhiteSpace(genderEntry.Text) && !string.IsNullOrWhiteSpace(weightEntry.Text)
                && !string.IsNullOrWhiteSpace(shoeSizeEntry.Text))
            {
                //var tempPatientID = db.Query<Patient>("SELECT PatientID FROM Patient");
                
                /*if(tempPatientID != null)
                {
                    await DisplayAlert("Hallo", "SUCCESS?", "Done");
                    await Navigation.PushAsync(new DatabaseQuery());
                }*/

                Patient patient = new Patient()
                {
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
                    await DisplayAlert("Add Patient: Error", "Invalid entry for age", "Done");
                }
                else if (!patient.isHeightWithinRange())
                {
                    await DisplayAlert("Add Patient: Error", "Invalid entry for height", "Done");
                }
                else if (!patient.isWeightWithinRange())
                {
                    await DisplayAlert("Add Patient: Error", "Invalid entry for weight", "Done");
                }
                else if (!patient.isShoeSizeWithinRange())
                {
                    await DisplayAlert("Add Patient: Error", "Invalid entry for shoe size", "Done");
                }
                else
                {
                    db.Insert(patient);
                    bool response = await DisplayAlert("Add Patient: Added", "Patient successfully added", "Next: Test Page", "Add another patient");
                    if(response)
                    {
                        await Navigation.PushAsync(new TestPage(patient));
                    }
                }
            }
            else
                await DisplayAlert("Add Patient: Error", "One or more fields missing information", "Done");
        }

        void helpButtonClicked(object sender, EventArgs e)
        {
            string helpMessage = "Purpose: Input the patient's ID, name, age, gender, weight, height and shoe size\n" +
                "Gender: Female, Male, Decline to State\n" +
                "Weight: Measured in pounds (lb)\n" +
                "Height: Measured in feet and inches (ft.in)\n" +
                "Shoe Size: Uses the US Shoe Size";
            DisplayAlert("Help - Add Patient Page", helpMessage, "Done");
        }
    } 
}