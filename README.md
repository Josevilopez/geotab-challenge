# Geotab Challenge

 This is a Geotab API program to download vehicle mileage and position to a CSV or XML.

Steps:

 1) Create Geotab API object from supplied arguments and authenticate.
 2) Get the odometer readings and gps position of each device and set a VehiculeData object, each minute
 3) Output the information to a CSV or XML file.
 4) Ctrl + C to finish

## Prerequisites

The sample application requires:

- [.Net core 2.0 SDK](https://dot.net/core) or higher

## Getting started

```shell
> git clone https://github.com/Josevilopez/geotab-challenge.git geotab-challenge
> cd geotab-challenge
> dotnet run "my.geotab.com" "database" "user@email.com" "password" ".csv"
```
