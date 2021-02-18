# Swimclub REST API & Mobile App
[![.NETBuild](https://github.com/K-Karol/Swimclub/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/K-Karol/Swimclub/actions/workflows/dotnet.yml)

## Introduction

This repository contains the project for the Sheffield Hallam GSDP group assignment.

Currently this code is not completely production ready, can be deployed on the local network.

[Trello Link](https://trello.com/b/dCStQ8XD/dronfield-swimming-club)

This project is built on the .NET platform. The REST API is built on the .NET 5, while the Xamarin app is developed in the .NET Standard 2.0 (with .NET 6, Xamarin is unified into MAUI).

The SwimclubStandard library (Swimclub namepspace) allows for code interoperability between the ASP.NET code and Xamarin code, therefore the code can be reused between the 2 platforms. .NET 5 is backwards compatitable with the .NET Standard 2.0.

## Architecture

This project is built with Visual Studio 2019. It uses the following major technologies:

- .NET 5 & .NET Standard 2.0
- ASP.NET
  - ASP.NET Entity Framework Core
  - ASP.NET Identity
- Xamarin
  - Programed using MVVM
- SQLite and SQLCipher (256-bit AES full database encryption)
  - Database files are fully encrypted and are protected by the REST API behind both Authentication and Authorization.

### SwimclubStandard

This project has all of the interoperable code that both the REST API server and the mobile app can use, such as data models and server responses. This method allows the data to be serialised on one end, and easily deserialsed on the other into the same data type.
Thanks to this, it speeds up development as it saves us creating special parsers between different programming languages / platforms, but also additionally it measn there is only 1 data type definition.

### Swimclub.REST

This is the source code for the REST API that is powering the whole project. This API is responsible for acting as an endpoint and service for data requests from the mobile app / any client (allows for expansion).

The API is secured using the JWT claim token scheme over secured SSL traffic (`https`). All transcations with the server (apart from the `/auth` endpoint) require the user to be authenticated from the relevant endpoint.
Authentication is peformed using the JWT scheme, where after sucessfull authentication, the client is presented with a bearer token used for all future requests till the token expires.
Additionally, requests that require authorization are checked against the set security policies using the current user's claims such as roles.

All of the responses from the API are standardized with the `Swimclub.Models.ServerResponse` abstract class. Each endpoint may declare an implementation of this class with additional data, which is also standardized. The `ServerResponse` has the public data member `ApiError` which carries all the information needed for debugging, with additional `Swimclub.Models.ServerResponse.ErrorCodes` so the client has more information on how to proceed with the error, while leaving the body of the `ApiError` human readable for testing.

For development purposes, the API comes with [Swagger built in](https://swagger.io/tools/swagger-ui/) for API testing purposes and documentation.

### Swimclub.Mobile (and Swimclub.Droid & Swimclub.iOS)

These projects are part of the Xamarin framework. The code in the `Mobile` project and namespace is the cross platform code. In the `Mobile` project, all of the page view and viewmodel code is located, with all of the logic.

The platform specific project (iOS as of right now is not developed or tested) contains the code to launch the app on the respected platform and load the `Swimclub.Mobile` code, as well as inject the platform specific services (implementations of service interfaces) to be used in the main cross-platform project.

### Data

All data is stored using [SQLite database engine](https://www.sqlite.org/index.html), or rather its [.NET implementation](https://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki) and is fully secured using the [SQLCipher extension](https://www.zetetic.net/sqlcipher/), therefore all of the data that is handeled is encrypted to enforce security.

Any API access to the data is strictly handled both on the client side as well as the server side. All references to the databases and strictly controlled in the code flow itself, whilst all procedures that access the database for client requests are checked for authentication by the ASP.NET Identity platform itself (therefore the request won't be even called on the API side as the ASP.NET will automatically reject the https request), while authorization is checked based on the policy the endpoint has. All users registered have a role assigned to them therefore these roles are checked against the policy to protected against malicious actions by the system users.
