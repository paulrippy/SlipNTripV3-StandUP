using SlipNTrip.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Xamarin.Forms;


namespace SlipNTrip
{
    public class DeviceControlsPage : ContentPage
    {
        private Patient patient;

        private Label testLabel;
        private Entry testEntry;

        private Label directionLabel;
        private Entry directionEntry;

        private Label distanceLabel;
        private Entry distanceEntry;

        private Label velocityLabel;
        private Entry velocityEntry;

        private Button runButton;
        private Button returnHome;

        public DeviceControlsPage(Patient patient)
        {
            AttributeValues attributeValues = new AttributeValues();

            this.patient = patient; 

            this.Title = "Device Controls";

            ToolbarItem helpToolbarItem = new ToolbarItem
            {
                Text = "?",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            helpToolbarItem.Clicked += helpButtonClicked;
            this.ToolbarItems.Add(helpToolbarItem);

            StackLayout stackLayout = new StackLayout();

            testLabel = new Label
            {
                Text = "Test Name",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(testLabel);
            testEntry = new Entry
            {
                Placeholder = "Test #0",
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(testEntry);

            directionLabel = new Label
            {
                Text = "Direction",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(directionLabel);
            directionEntry = new Entry
            {
                Placeholder = "Forward/Backward",
                FontSize = attributeValues.getEntryFontSize()
            };
            stackLayout.Children.Add(directionEntry);

            distanceLabel = new Label
            {
                Text = "Distance",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(distanceLabel);
            distanceEntry = new Entry
            {
                Placeholder = "Select between 0 and 15 cm",
                FontSize = attributeValues.getEntryFontSize(),
                Keyboard = Keyboard.Numeric
            };
            stackLayout.Children.Add(distanceEntry);

            velocityLabel = new Label
            {
                Text = "Velocity",
                FontSize = attributeValues.getLabelFontSize()
            };
            stackLayout.Children.Add(velocityLabel);
            velocityEntry = new Entry
            {
                Placeholder = "Select between 15 and 35 cm/s",
                FontSize = attributeValues.getEntryFontSize(),
                Keyboard = Keyboard.Numeric
            };
            stackLayout.Children.Add(velocityEntry);

            runButton = new Button
            {
                Text = "Run",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            runButton.Clicked += runButtonClicked;
            stackLayout.Children.Add(runButton);

            returnHome = new Button
            {
                Text = "Return to Home Page",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            returnHome.Clicked += returnHomeButtonClicked;
            stackLayout.Children.Add(returnHome);

            Content = stackLayout;
        }

        async void runButtonClicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(testEntry.Text) && !string.IsNullOrWhiteSpace(directionEntry.Text) &&
                !string.IsNullOrWhiteSpace(distanceEntry.Text) && !string.IsNullOrWhiteSpace(velocityEntry.Text))
            {
                if (directionEntry.Text != "forward" && directionEntry.Text != "Forward"
                    && directionEntry.Text != "f" && directionEntry.Text != "F"
                    && directionEntry.Text != "backward" && directionEntry.Text != "Backward"
                    && directionEntry.Text != "b" && directionEntry.Text != "B")
                {
                    await DisplayAlert("Device Controls Error", "Incorrect input for direction", "Done");
                }
                else if (double.Parse(distanceEntry.Text) < 0 || double.Parse(distanceEntry.Text) > 15)
                {
                    await DisplayAlert("Device Controls Error", "Input for distance is out of range", "Done");
                }
                else if (double.Parse(velocityEntry.Text) < 15 || double.Parse(velocityEntry.Text) > 35)
                {
                    await DisplayAlert("Device Controls Error", "Input for velocity is out of range", "Done");
                }
                else
                {
                    TestResults testResults = new TestResults()
                    {
                        PatientName = patient.Name,
                        PatientID = patient.ID,
                        TestName = testEntry.Text,
                        Date = DateTime.Now,
                        Direction = directionEntry.Text,
                        Distance = double.Parse(distanceEntry.Text),
                        MotorSpeed = double.Parse(velocityEntry.Text),
                        StepTaken = false,
                        TimeBetweenStep = DateTime.Now - DateTime.Now,
                        DistanceBetweenStep = 0.0
                    };

                    //using (var client = new UdpClient())
                    //{
                    //    client.EnableBroadcast = true;
                    //    var endpoint = new IPEndPoint(IPAddress.Broadcast, 4210);
                    //    var message = Encoding.ASCII.GetBytes(distanceEntry.Text);
                    //    await client.SendAsync(message, message.Length, endpoint);
                    //    client.Close();
                    //
                    //} 
                    await Navigation.PushAsync(new RunPage(patient, testResults, false));
                }
            }
            else
                await DisplayAlert("Device Controls Error", "One or more fields missing information", "Done");
        }
          

        async void returnHomeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

        void helpButtonClicked(object sender, EventArgs e)
        {
            string helpMessage = "Direction: Forward/Backward\n" +
                "Distance: 0 cm to 15 cm\n" +
                "Velocity/Speed: 15cm/s to 35cm/s";
            DisplayAlert("Help - Device Controls Page", helpMessage, "Done");
        }
    }
}