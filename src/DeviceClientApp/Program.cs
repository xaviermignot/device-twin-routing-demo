using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Configuration;

namespace DeviceClientApp
{
    class Program
    {
        private static DeviceClient _deviceClient;

        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            var deviceConnectionString = configuration["DeviceConnectionString"];
            _deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString);

            var tokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (_, _) =>
            {
                Console.WriteLine("Cancellation requested !");
                tokenSource.Cancel();
            };

            await StartDeviceAsync(tokenSource.Token);
        }

        private static async Task StartDeviceAsync(CancellationToken cancellationToken)
        {
            var twinCollection = new TwinCollection();
            twinCollection["LastStartDate"] = DateTime.UtcNow.ToString("s");
            Console.WriteLine("Updating device last start date...");
            await _deviceClient.UpdateReportedPropertiesAsync(twinCollection);

            var blueColor = false;
            while (!cancellationToken.IsCancellationRequested)
            {
                await UpdateDeviceColorAsync(blueColor ? "Blue" : "Red");
                blueColor = !blueColor;
                await Task.Delay(3000);
            }
        }

        private static Task UpdateDeviceColorAsync(string color)
        {
            var twinCollection = new TwinCollection();
            twinCollection["Color"] = color;
            Console.WriteLine($"Set device color to {color}");
            return _deviceClient.UpdateReportedPropertiesAsync(twinCollection);
        }
    }
}