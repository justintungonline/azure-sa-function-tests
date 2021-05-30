# Azure Stream Analytics Function Tests

The repository is for testing Azure Stream Analytics working with Azure Functions and following steps in this [Microsoft Tutorial: Run Azure Functions from Azure Stream Analytics jobs](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-with-azure-functions)

[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/justintungonline/azure-sa-function-tests)

## Tips and Workarounds Used to Complete the Tutorial

List of things I had to choose in the tutorial where the tutorial doesn't say exact steps or options to follow.

### Create a Stream Analytics job with Event Hubs as input

[Tutorial: Analyze fraudulent call data with Stream Analytics and visualize results in Power BI dashboard](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-real-time-fraud-detection)

- Stopped at "Configure job output section describing connecting to PowerBI to visualize data". Can continue tutorial learn about PowerBI integration.
- When configuring the Stream Analytics input, select connection string and then the Azure Event Hub policy name you created.

### Create an Azure Cache for Redis instance

[Quickstart: Use Azure Cache for Redis in .NET Framework](https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-dotnet-how-to-use-azure-redis-cache#create-a-cache)

1. During creation, use a public endpoint to avoid costs with VNets and private endpoints.
2. Used V4
3. In my tutorial, redis cache took 20+ minutes to create

### Create a function in Azure Functions that can write data to Azure Cache for Redis

- Created function using the command line interface (CLI) tutorial called [Quickstart: Create a C# function in Azure from the command line](https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Ccurl) using the Gitpod development environment in this repository. [Launch the online development environment](https://gitpod.io/#https://github.com/justintungonline/azure-sa-function-tests)
    - The `HttpExample.cs` is modified from the tutorial to fit a C# implementation instead of a C# script.
    - Azure Functions and Redis cache has a [known issue](https://github.com/StackExchange/StackExchange.Redis/issues/1655) where this line below needs to be added to the `.csproj` file . Without the line, there will be a `Could not load file or assembly 'System.IO.Pipelines` error when the function tries to get a connection.

    ```xml
    <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <AzureFunctionsVersion>v3</AzureFunctionsVersion>
        <_FunctionsSkipCleanOutput>true</_FunctionsSkipCleanOutput> <!-- *** this line was added *** -->
    </PropertyGroup>
    ```
- The Gitpod environment is this repository has all the prerequisites for the CLI tutorial (dotnetcore, Azure CLI, Azure Function Tools Core ) preinstalled in the workspace's Ubuntu image. No local installs are required. The image has been tested with the steps in the tutorial to build the function and deploy it to Azure.
- Dependencies to install StackExchange.Redis and Functions using dotnet CLI in project

```sh

# https://www.nuget.org/packages/StackExchange.Redis/
dotnet add package StackExchange.Redis --version 2.2.4

# https://www.nuget.org/packages/Microsoft.NET.Sdk.Functions/
dotnet add package Microsoft.NET.Sdk.Functions --version 3.0.11

```

[Reference of files generated for the Function App](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp#generated-project-files)

Helpful commands

```sh
# Build and run function
func start --csharp

...

# Login to Azure
az login

# Set subscription
az account set --subscription <subscription>

# Create Azure Function app with existing resource group and storage account I created in advance of creating the function, then publish
# functions-version will determine the dotnet version
az functionapp create --resource-group <my-resource-group-name> --consumption-plan-location canadacentral --functions-version 3 --name sandboxfunction1 --storage-account sandboxstorageaccount

# Publish c# function
func azure functionapp publish sandboxfunction1 --csharp
```
