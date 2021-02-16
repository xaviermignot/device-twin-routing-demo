# Device twin routing demo

This repository contains the code of a simulated device interacting with an Azure IoT Hub.  
It has been built for a [blog post](https://blog.xmi.fr/posts/iot-hub-device-twin-routing-deep-dive/) I have written on IoT Hub message routing for device twin change events. 

## How to get started with this ?
To run this on your machine follow this steps:
1. Install the [.NET 5 SDK](https://dotnet.microsoft.com/download) if you don't already have it
2. Create an Azure IoT Hub on your subscription in the Free or Standard Tier
3. Create a device in your hub, with SAS authentication, and copy it connection string
4. Clone the repo, go in the `src/DeviceClientApp` folder and set the connection string with the following command: `dotnet user-secrets set "DeviceConnectionString" "<YOUR-DEVICE-CONNECTION-STRING>"`
5. You're ready to launch it using the `dotnet run` command !

## What does it do ?
The first time the program is run for a device, it should give the following output:
```console
$ dotnet run
DeviceClient created ! Getting current twin...
First time with this device, let's initialize it !
Done ! Exiting now...
```
This first run creates two reported properties:
- `propertyUsedOnlyOnce` which will never be used again 😔
- `color` set to `Red`

The next runs of the tool will change the value of the `color` property to `Blue`, then back to `Red`, then back to `Blue`, and so on...  
But why ? Well, it's just for a demo, read the [blog post](https://blog.xmi.fr/posts/iot-hub-device-twin-routing-deep-dive/) to find out more !