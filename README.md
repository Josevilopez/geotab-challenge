# Geotab Challenge

 This example demonstrates how to get mileage for all vehilces using the MyGeotab API and save to a CSV or XML file.

Steps:

 1) Create Geotab API object from supplied arguments and authenticate.
 2) Get the odometer readings and position of each device and set a  VehiculeData object.
 3) Output the information to a CSV file.

## Prerequisites

The sample application requires:

- [.Net core 2.0 SDK](https://dot.net/core) or higher

## Getting started

```shell
> git clone https://github.com/Josevilopez/geotab-challenge.git geotab-challenge
> cd geotab-challenge
> cd ExtractMileage
> dotnet run "my.geotab.com" "database" "user@email.com" "password" ".csv"
```
