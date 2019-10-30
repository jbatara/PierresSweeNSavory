# _Pierre's Sweet N Savory_

#### _A Basic Online Baked Treats and Flavor Tracker_

#### _By **Jennifer Batara**_

## Description

This application is a ASP.Net MVC web application track treats in a bakery and what flavor profiles they have. Each flavor can also be clicked on to see which treats are associate with it.

Signed in users are able to add treats and flavors, and add relationships between these two items.


## Setup/Installation Requirements

-   Internet Connection
-   Internet browser
-   Bash Terminal
-   .NET Core 2.2
-   SQLite

If you do not have the .NET Core installed on your computer, please install it by following the directions for your operating system [here](https://dotnet.microsoft.com/download). The .NET Core version used for this project is 2.2.

To view locally please copy the link to [this repo](https://github.com/jbatara/SweetNSavory) and type the following command into your Bash terminal:
```
$git clone repo_url
```

with repo_url being the url that was just copied. To open the console app, navigate to the local directory which the online repository was cloned to using the command

```
$cd SweetNSavory/SweetNSavory
```

Once in the correct repository, and confirming that you have .NET core installed (version 2.2), run the app with the command
```
$dotnet run
```
and enjoy!

In the event that the SQLite database is not available, create an app.db file in the SweetNSavory/SweetNSavory/ folder and run the command
```
$dotnet ef database update
```
to update your local SQLite database for the correct specifications.

This project is currently not hosted online.

## Known Bugs

- When registering a user, the error "No such column u.Discriminator"

## Support and contact details

Please feel free to contact the developer by raising a new [issue](https://github.com/jbatara/SweetNSavory/issues/new) on the github repo. You can browse the current issues [here](https://github.com/jbatara/SweetNSavory/issues).

## Technologies Used

* C#
* .NET Core 2.2

### License

_MIT_

Copyright (c) 2019 **_Jennifer Batara_**
