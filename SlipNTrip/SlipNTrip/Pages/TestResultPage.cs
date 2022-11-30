using SlipNTrip.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SQLite;
using Xamarin.Forms;

namespace SlipNTrip.Pages
{
    public class TestResultPage : ContentPage
    {
        string dbTestResultsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "testResults.db3");

        private Patient patient;
        private TestResults testResults;
        private Boolean buttonLayout;

        private int arraySize = 11;
        private double[,] startingLocationArray;
        private double[,] endingLocationArray;

        private Label patientID;
        private Label patientName;
        private Label testName;
        private Label testDate;
        private Label patientGender;
        private Label patientAge;
        private Label patientHeight;
        private Label patientWeight;
        private Label patientShoeSize;
        private Label deviceDirection;
        private Label deviceDistance;
        private Label deviceVelocity;
        private Label stepTaken;
        private Label timeBetweenStep;
        private Label distanceBetweenStep;

        private Button steppingSurfaceGraphicButton;
        private Button saveButton;
        private Button newTestButton;
        private Button homeButton;
        private Button exportButton;
        private Button deleteButton;

        public TestResultPage(Patient patient, TestResults testResults, Boolean buttonLayout)
        {
            AttributeValues attributeValues = new AttributeValues();

            this.patient = patient;
            this.testResults = testResults;
            this.buttonLayout = buttonLayout;
            this.Title = patient.PatientID + ": " + patient.Name + " - " + testResults.TestName;

            //generateArray_1(); // For Testing (random example)
            generateArray_2(); // For Testing (Foot example)

            ToolbarItem helpToolbarItem = new ToolbarItem
            {
                Text = "?",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            helpToolbarItem.Clicked += helpButtonClicked;
            this.ToolbarItems.Add(helpToolbarItem);

            StackLayout stackLayout = new StackLayout();

            patientID = new Label();
            patientID.Text = "Patient ID: " + patient.PatientID;
            patientID.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(patientID);

            patientName = new Label();
            patientName.Text = "Patient Name: " + patient.Name;
            patientName.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(patientName);

            testName = new Label();
            testName.Text = "Test Name: " + testResults.TestName;
            testName.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(testName);

            testDate = new Label();
            testDate.Text = "Date: " + testResults.Date.ToString();
            testDate.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(testDate);

            patientGender = new Label();
            patientGender.Text = "Gender: " + patient.Gender;
            patientGender.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(testDate);

            patientAge = new Label();
            patientAge.Text = "Age: " + patient.Age.ToString();
            patientAge.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(patientAge);

            patientHeight = new Label();
            patientHeight.Text = "Height: " + patient.Height.ToString();
            patientHeight.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(patientHeight);

            patientWeight = new Label();
            patientWeight.Text = "Weight: " + patient.Weight.ToString();
            patientWeight.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(patientWeight);

            patientShoeSize = new Label();
            patientShoeSize.Text = "Shoe Size: " + patient.ShoeSize.ToString();
            patientShoeSize.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(patientShoeSize);

            deviceDirection = new Label();
            deviceDirection.Text = "Direction: " + testResults.Direction;
            deviceDirection.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(deviceDirection);

            deviceDistance = new Label();
            deviceDistance.Text = "Distance: " + testResults.Distance.ToString();
            deviceDistance.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(deviceDistance);

            deviceVelocity = new Label();
            deviceVelocity.Text = "Velocity/Speed: " + testResults.MotorSpeed.ToString();
            deviceVelocity.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(deviceVelocity);

            stepTaken = new Label();
            stepTaken.Text = "Was a step taken? " + testResults.WasAStepTaken();
            stepTaken.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(stepTaken);

            timeBetweenStep = new Label();
            timeBetweenStep.Text = "Time between steps: " + testResults.TimeBetweenStep.ToString();
            timeBetweenStep.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(timeBetweenStep);

            distanceBetweenStep = new Label();
            distanceBetweenStep.Text = "Distance between steps: " + testResults.DistanceBetweenStep.ToString();
            distanceBetweenStep.FontSize = attributeValues.getLabelFontSize();
            stackLayout.Children.Add(distanceBetweenStep);

            steppingSurfaceGraphicButton = new Button
            {
                Text = "View Patient's Location on Device",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            steppingSurfaceGraphicButton.Clicked += steppingSurfaceGraphicButtonClicked;
            stackLayout.Children.Add(steppingSurfaceGraphicButton);

            saveButton = new Button
            {
                Text = "Save",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            saveButton.Clicked += SaveButtonCLicked;
            stackLayout.Children.Add(saveButton);

            newTestButton = new Button
            {
                Text = "New Test",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            newTestButton.Clicked += NewTestButtonCLicked;
            stackLayout.Children.Add(newTestButton);

            exportButton = new Button
            {
                Text = "Export",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            exportButton.Clicked += ExportButtonCLicked;
            stackLayout.Children.Add(exportButton);

            deleteButton = new Button
            {
                Text = "Delete",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            deleteButton.Clicked += DeleteButtonCLicked;
            stackLayout.Children.Add(deleteButton);

            homeButton = new Button
            {
                Text = "Return to Home Page",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            homeButton.Clicked += HomeButtonCLicked;
            stackLayout.Children.Add(homeButton);

            if (buttonLayout)
            {
                saveButton.IsVisible = false;
                newTestButton.IsVisible = false;
                deleteButton.IsVisible = true;
            }
            else
            {
                saveButton.IsVisible = true;
                newTestButton.IsVisible = true;
                deleteButton.IsVisible = false;
            }
            
            Content = stackLayout;
            //ScrollView scrollView = new ScrollView { Content = stackLayout };
        }

        async void steppingSurfaceGraphicButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SSGraphicPage(startingLocationArray, endingLocationArray));
        }

        async void SaveButtonCLicked(object sender, EventArgs e)
        {
            var dbTestResults = new SQLiteConnection(dbTestResultsPath);
            dbTestResults.CreateTable<TestResults>();

            dbTestResults.Insert(testResults);

            await DisplayAlert("Save", "Results Sucessfully Saved", "Done");
        }

        async void NewTestButtonCLicked(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("New Test", "Did you save the results?", "Yes", "No");
            if(response)
            {
                await Navigation.PushAsync(new TestPage(patient));
            }
        }

        async void ExportButtonCLicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Export To?", "Cancel", null, "Text File", "Excel File", "PDF");
            if(action == "Test File")
            {
                //string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");
                //File.WriteAllText(fileName, "Hello");
            }
            else if(action == "Excel File")
            {

            }
            else if(action == "PDF")
            {

            }
        }

        async void DeleteButtonCLicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbTestResultsPath);
            bool response = await DisplayAlert("Delete: Test Result", "Are you sure you would like to delete the test result?", "Yes", "No");
            if (response)
            {
                db.Table<TestResults>().Delete(x => x.ID == this.testResults.ID);
                await Navigation.PopAsync();
            }
        }

        async void HomeButtonCLicked(object sender, EventArgs e)
        {
            if(buttonLayout)
            {
                await Navigation.PushAsync(new HomePage());
            }
            else
            {
                bool response = await DisplayAlert("Return home", "Did you save the results?", "Yes", "No");
                if (response)
                {
                    await Navigation.PushAsync(new HomePage());
                }
            }
        }

        void helpButtonClicked(object sender, EventArgs e)
        {
            string helpMessage1 = "Purpose: To view the test results\n" +
                "The test results include patient information, device control information and data collected from the stepping surface\n" +
                "Save: Saves test results into database\n" +
                "New Test: Navigates to the test page\n" +
                "Export: Exports data into desired file format (text file, excel file, PDF)\n" +
                "Home Page: Navigates to home page";
            string helpMessage2 = "Purpose: To view the test results\n" +
                "The test results include patient information, device control information and data collected from the stepping surface\n" +
                "Export: Exports data into desired file format (text file, excel file, PDF)\n" +
                "Delete: Removes test results from database";
            if(buttonLayout)
            {
                DisplayAlert("Help - Test Results Page", helpMessage2, "Done");
            }
            else
            {
                DisplayAlert("Help - Test Results Page", helpMessage1, "Done");
            }
        }

        // For Testing
        private void generateArray_1()
        {
            startingLocationArray = new double[arraySize, arraySize];
            endingLocationArray = new double[arraySize, arraySize];
            Random rnd = new Random();
            for (int x = 0; x < arraySize; x++)
            {
                for (int y = 0; y < arraySize; y++)
                {
                    startingLocationArray[x, y] = rnd.Next(0, 1000);
                }
            }

            for (int x = 0; x < arraySize; x++)
            {
                for (int y = 0; y < arraySize; y++)
                {
                    endingLocationArray[x, y] = rnd.Next(0, 1000);
                }
            }
        }

        private void generateArray_2()
        {
            // min tomato (Small amount of pressure) = 200
            // min crimson = 400
            // min firebrick = 600
            // min dark red = 800
            startingLocationArray = new double[,] { 
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #1
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #2
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #3
                { 100, 100, 100, 100, 300, 700, 900, 100, 100, 100, 100}, // Row #4
                { 100, 100, 100, 100, 500, 300, 900, 100, 100, 100, 100}, // Row #5
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #6
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #7
                { 100, 100, 100, 100, 500, 300, 900, 100, 100, 100, 100}, // Row #8
                { 100, 100, 100, 100, 300, 700, 900, 100, 100, 100, 100}, // Row #9
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #10
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}  // Row #11
            };

            endingLocationArray = new double[,] {
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #1
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #2
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #3
                { 100, 100, 100, 100, 300, 700, 900, 100, 100, 100, 100}, // Row #4
                { 100, 100, 100, 100, 500, 300, 900, 100, 100, 100, 100}, // Row #5
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #6
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #7
                { 100, 100, 500, 300, 900, 100, 100, 100, 100, 100, 100}, // Row #8
                { 100, 100, 300, 700, 900, 100, 100, 100, 100, 100, 100}, // Row #9
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}, // Row #10
                { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100}  // Row #11
            };
        }
    }
}