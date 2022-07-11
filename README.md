# Frends.Tv4Salesforce

frends Community Task for BulkInsert

[![Actions Status](https://github.com/CommunityHiQ/Frends.Tv4Salesforce/workflows/PackAndPushAfterMerge/badge.svg)](https://github.com/CommunityHiQ/Frends.Tv4Salesforce/actions) ![MyGet](https://img.shields.io/myget/frends-community/v/Frends.Tv4Salesforce) [![License: UNLICENSED](https://img.shields.io/badge/License-UNLICENSED-yellow.svg)](https://opensource.org/licenses/UNLICENSED) 

- [Installing](#installing)
- [Tasks](#tasks)
     - [SendBulkDataAsBytes](#SendBulkDataAsBytes)
- [Building](#building)
- [Contributing](#contributing)

# Installing

You can install the Task via frends UI Task View or you can find the NuGet package from the following NuGet feed
https://www.myget.org/F/frends-community/api/v3/index.json and in Gallery view in MyGet https://www.myget.org/feed/frends-community/package/nuget/Frends.Tv4Salesforce

# Tasks

## SendBulkDataAsBytes

This Frends task performs insert/upsert of bulk data from CSV file. It hadles Salesforces bulk job from creation to closing.

### Properties

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| DirectoryCSV | `string` | Directory of CSV file to be sent to Tv4Salesforce  | `C:\` |
| BaseDomainURL | `string` | Base domain URL used to send requests to Tv4Salesforce | `https://example-domain.salesforce.com` |
| AuthToken | `string` | Authorization token | ` Bearer 1234567890abcdefghijklmnopqrstuvwxyz` |
| CreateJobBody | `string` | Body describing job settings | `"{\"object\" : \"example_entity\",\"contentType\" : \"CSV\",\"operation\" : \"insert\",\"lineEnding\" : \"CRLF\"}"` |


### Returns

A result is json serialization of object with parameters.

| Property | Type | Description | Example |
| -------- | -------- | -------- | -------- |
| HttpResultBody | `string` | Body containing information of finished job | `` |

# Building

Clone a copy of the repository

`git clone https://github.com/CommunityHiQ/Frends.Tv4Salesforce.git`

Rebuild the project

`dotnet build`

Create a NuGet package

`dotnet pack --configuration Release`

# Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repository on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

