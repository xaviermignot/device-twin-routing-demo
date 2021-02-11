using System;
using System.Reflection;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

var deviceConnectionString = configuration["DeviceConnectionString"];

if (string.IsNullOrEmpty(deviceConnectionString))
{
    Console.WriteLine("Device connection string not found !");
    Console.WriteLine("Run dotnet user-secrets set DeviceConnectionString '<DEVICE CONNECTION STRING>'");
    return;
}

var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);

Console.WriteLine("DeviceClient created ! Getting current twin...");
var currentTwin = await deviceClient.GetTwinAsync();
var currentColor = currentTwin.Properties.Reported.Contains("color") ? currentTwin.Properties.Reported["color"] : null;
var newColor = currentColor == "Blue" ? "Red" : "Blue";

var twinUpdate = new TwinCollection();
twinUpdate["color"] = newColor;

Console.WriteLine($"Changing 'color' reported property from '{currentColor}' to '{newColor}'...");
await deviceClient.UpdateReportedPropertiesAsync(twinUpdate);
Console.WriteLine("Done ! Exiting now...");