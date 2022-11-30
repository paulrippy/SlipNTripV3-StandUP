using SlipNTrip.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using System.Net;
using System.Net.Sockets;
using Xamarin.Forms;
using System.IO;
using System.IO.Ports;
using System.Collections;

/*
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;

// using Plugin.BluetoothLE;
// using Plugin.BluetoothLE.Server;
*/

namespace SlipNTrip.Pages
{
    public class RunPage : ContentPage
    {
        private Patient patient;
        private TestResults testResults;
        private bool buttonLayout;

        private Button resultButton;

        public DateTime start;
        // private object _udpClient; // for Wifi UDP client. Used on V2 of project

        /*
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        IDevice device;

        private Button statusButton;
        private Button scanButton;
        private Button connectButton;
        
        double encDirection;
        byte serialDirection;
        byte serialDistance;
        byte serialVelocity;
        double veloAdder = 100.0;
        */
        public RunPage(Patient patient, TestResults testResults, bool buttonLayout)
        {

            this.patient = patient;
            this.testResults = testResults;
            this.buttonLayout = buttonLayout;

            AttributeValues attributeValues = new AttributeValues();
            start = DateTime.Now;



            /*
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
            */

            // if (testResults.Direction == "forward" || testResults.Direction == "Forward"
            //   || testResults.Direction == "f" || testResults.Direction == "F")
            // {
            //   encDirection = 255.0;
            // }
            // else if (testResults.Direction == "backward" || testResults.Direction == "Backward"
            //   || testResults.Direction == "b" || testResults.Direction == "B")
            // {
            //     encDirection = 254.0;
            // }

            //var encDistance = testResults.Distance;
            //var encVelocity = testResults.MotorSpeed + veloAdder;
            //serialDirection = Convert.ToByte(encDirection);
            //serialDistance = Convert.ToByte(encDistance);
            //serialVelocity = Convert.ToByte(encVelocity);
            //serialSend[0] = serialDirection;
            //Console.Write("hello world");
            // serialSend[1] = serialDistance;
            // serialSend[2] = serialVelocity;

            // SerialDevice port = new SerialDevice("/dev/tty.usbmodem141101", BaudRate.B9600);
            // port.Open();
            //port.Write(serialSend);
            // port.Close();
            
            Title = "Test in Progress.....";
            StackLayout stackLayout = new StackLayout();

            ToolbarItem helpToolbarItem = new ToolbarItem
            {
                Text = "?",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };
            helpToolbarItem.Clicked += helpToolbarItemClicked;
            this.ToolbarItems.Add(helpToolbarItem);

            /*
            statusButton = new Button
            {
                Text = "Check Bluetooth Status",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            statusButton.Clicked += btnStatusClicked;
            stackLayout.Children.Add(statusButton);

            scanButton = new Button
            {
                Text = "Scan for Motor Controller",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            scanButton.Clicked += btnScanClicked;
            stackLayout.Children.Add(scanButton);

            connectButton = new Button
            {
                Text = "Connect to Motor Controller",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            connectButton.Clicked += btnConnectClicked;
            stackLayout.Children.Add(scanButton);
            */

            resultButton = new Button
            {
                Text = "See Test Results",
                FontSize = attributeValues.getLabelFontSize(),
                BorderColor = Color.DarkGray,
                BorderWidth = attributeValues.getBorderWidth()
            };
            resultButton.Clicked += navigateToTestPage;
            stackLayout.Children.Add(resultButton);

            ImageButton emergencyStopButton = new ImageButton
            {
                Source = "Emergency_Stop.png",
                HorizontalOptions = LayoutOptions.Center
            };
            emergencyStopButton.Clicked += emergencyStopClicked;
            stackLayout.Children.Add(emergencyStopButton);
            Content = stackLayout;

        }


        /*
        private void btnStatusClicked(object sender, EventArgs e)
        {
            var state = ble.State;
            this.DisplayAlert("Notice", state.ToString(), "Done");
        }

        private async void btnScanClicked(object sender, EventArgs e)
        {
            try
            {
                deviceList.Clear();
                adapter.DeviceDiscovered += (s, a) =>
                {
                    deviceList.Add(a.Device);
                    if(a.Device.Name == "Long name works now")
                    {
                        device = a.Device;
                    }

                };

                //We have to test if the device is scanning 
                if (!ble.Adapter.IsScanning)
                {
                    await adapter.StartScanningForDevicesAsync();

                }
                if(ble.Adapter.IsScanning)
                    await DisplayAlert("Notice", "iPad is scanning", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Notice", ex.Message.ToString(), "Error !");
            }
        }

        private async void btnConnectClicked(object sender, EventArgs e)
        {
            try
            {
                if (device != null)
                {
                    await adapter.ConnectToDeviceAsync(device);
                    await DisplayAlert("Notice", "ESP32 Connected", "OK");
                }
                else
                {
                    await DisplayAlert("Notice", "ESP32 Not Connected", "OK");
                }
            }
            catch (DeviceConnectionException ex)
            {
                //Could not connect to the device
                await DisplayAlert("Notice", ex.Message.ToString(), "OK");
            }

        }
        */


        async void helpToolbarItemClicked(object sender, EventArgs e)
        {
            string helpMessage = "Purpose: Shows test is in progress & displays emergency stop\n" + 
                "Page will automatically navigate to test results page when test is complete\n" +
                "Emergency Stop will navigate back to device controls page";
            await DisplayAlert("Help - Test in progress.....", helpMessage, "Done");
        }


        async void emergencyStopClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Emergency Stop", "Emergency Stop Engaged", "Done");
            //await Navigation.PushAsync(new TestResultPage(patient, testResults, false)); // For testing
            await Navigation.PushAsync(new DeviceControlsPage(patient));
        }

        async void navigateToTestPage(object sender, EventArgs e)
        {
            var elapsed = DateTime.Now - start;
            testResults.TimeBetweenStep = elapsed;
            bool step = await DisplayAlert("Step Status", "Did the patient take a step?", "Yes", "No");
            testResults.StepTaken = step;
            await Navigation.PushAsync(new TestResultPage(patient, testResults, false));
        }
        



            // Defying Gravity V2 Emergency stop and Navigate to test page using Wi-Fi UDP
            /*
            async void emergencyStopClicked(object sender, EventArgs e)
            {
                using (var client = new UdpClient())
                {
                    client.EnableBroadcast = true;
                    var endpoint = new IPEndPoint(IPAddress.Broadcast, 4210);
                    var message = Encoding.ASCII.GetBytes("-999");
                    await client.SendAsync(message, message.Length, endpoint);
                    client.Close();
                }
                await DisplayAlert("Emergency Stop", "Emergency Stop Engaged", "Done");
                //await Navigation.PushAsync(new TestResultPage(patient, testResults, false)); // For testing
                await Navigation.PushAsync(new DeviceControlsPage(patient));
            }

            async void navigateToTestPage()
            {
                UdpClient _udpClient = new UdpClient(4210);
                string message;
                do
                {
                    var result = await _udpClient.ReceiveAsync();
                    message = Encoding.ASCII.GetString(result.Buffer);
                    //Console.WriteLine(message);
                } while (message.Length == 0);

                if(message.Contains("-999"))
                {
                    await DisplayAlert("Emergency Stop", "Manual Emergency Stop Engaged", "Done");
                    await Navigation.PushAsync(new DeviceControlsPage(patient));
                }
                else
                {
                    await Navigation.PushAsync(new TestResultPage(patient, testResults, false));
                }
            }
            */
     }
}