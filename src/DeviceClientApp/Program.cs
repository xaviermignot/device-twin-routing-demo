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

var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString);

Console.WriteLine("DeviceClient created ! Getting current twin...");
var currentTwin = await deviceClient.GetTwinAsync();

var twinUpdate = new TwinCollection();
if (!currentTwin.Properties.Reported.Contains("color"))
{
    Console.WriteLine("First time with this device, let's initialize it !");
    twinUpdate["propertyUsedOnlyOnce"] = "This is set only once";
    twinUpdate["color"] = "Red";
}
else
{
    var currentColor = currentTwin.Properties.Reported["color"];
    var newColor = currentColor == "Blue" ? "Red" : "Blue";
    twinUpdate["color"] = newColor;
    Console.WriteLine($"Changing 'color' reported property from '{currentColor}' to '{newColor}'...");
}

await deviceClient.UpdateReportedPropertiesAsync(twinUpdate);
Console.WriteLine("Done ! Exiting now...");