using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.IoT.Devices;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.UI.Popups;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random random = new Random();
         
        public MainPage()
        {
            this.InitializeComponent();

           
        }

        // Capture user input for tempurature value then call function to send value to IoT Central
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.TextBlock1.Text = "Temp value sent: "+ this.TextBox1.Text;
            try
            {
                await SendDeviceToCloudMessagesAsync(int.Parse(TextBox1.Text));
            }
            catch (Exception except)
            {

                var dialog = new MessageDialog("Error: " + except.Message);
                await dialog.ShowAsync();
                
            }
        }

        //Function to send telemetry data to IoT Central
        static async Task SendDeviceToCloudMessagesAsync(int temp)
        {
            string iotHubUri = "saas-iothub-10fa9ffe-b520-4554-af58-1a256ecdaf63.azure-devices.net"; // ! put in value !
            string deviceId = "127zzkd"; // ! put in value !
            string deviceKey = "kuNK8RfMq5C0jooJ+eP8nPg41tHAvxJlGop+O0415+M="; // ! put in value !

            var deviceClient = DeviceClient.Create(iotHubUri,
                AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
                TransportType.Http1);

            var telemetryDataPoint = new {deviceId, tmp1 = temp}; //create telemetry datapoint
            var messageString = JsonConvert.SerializeObject(telemetryDataPoint); //convert to JSON
            var message = new Message(Encoding.ASCII.GetBytes(messageString)); //Encode message 
            await deviceClient.SendEventAsync(message); //send message to Hub

        }
    }
}
