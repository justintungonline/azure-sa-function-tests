# Azure Stream Analytics Function Tests

The repository is for testing Azure Stream Analytics working with Azure Functions and following steps in this [Microsoft Tutorial: Run Azure Functions from Azure Stream Analytics jobs](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-with-azure-functions)

[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/justunsix/azure-sa-function-tests)

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

- Created function using the command line interface (CLI) tutorial called [Quickstart: Create a C# function in Azure from the command line](https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Ccurl) using the Gitpod development environment in this repository. [Launch the online development environment](https://gitpod.io/#https://github.com/justunsix/azure-sa-function-tests)
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

## Post Tutorial Improvements

### Automated Deployment to Azure with GitHub action

Follow [Continuous delivery by using GitHub Action](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-github-actions?tabs=dotnet) to use the [Azure Function Action](https://github.com/marketplace/actions/azure-functions-action).

This repository uses the [Linux dotnet function app template](https://github.com/Azure/actions-workflow-samples/blob/master/FunctionApp/linux-dotnet-functionapp-on-azure.yml).

### Security

This section goes through security improvements applied to the Azure Event Hubs > Stream Analytics > Azure Function data flow.

#### Set a budget and monitor

A budget can monitor the actual or forecast cost on the resources in the tutorial and notify you of issues. Here is an example to get alerts based on a spend:

1. In the resource group holding all the tutorial resources, go to Budgets.
2. Add a new budget and set a monthly forecast. If redis cache is turned off, the monthly forecast for the tutorial resources should be lower then $50. Set $50 as the monthly budget.
3. Set alerts for the budget. Optionally, set up an action group with notifications, actions, and tasks than can be perform in case an alert is generated.

#### Event Hubs

##### Network security

Using the network firewall in Event Hubs, restrict IPs to know callers of the Event Hub. For the tutorial, it should be your IP address or range and trusted Microsoft services like Stream Analytics. In the Event Hub resource:

1. Go to Networking > Firewalls and virtual networks
2. For **Allow access from**: Check `Selected networks`
3. In the firewall, enter your IP address or range. For example if your IP is `38.22.189.24`, you can enter that IP or range `38.22.189.0/24` to cover hosts in the 38.22.189.0 subnet.

###### Give access to Stream Analytics

1. Go to Networking > Firewalls and virtual networks
2. For **Allow trusted Microsoft services to bypass this firewall?** Check `Yes`.

- This setting ensures [Stream Analytics can connect](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-ip-filtering) to Event Hubs.
- The Stream Analytics job should be configured to use a managed identity to access the event hub. In the Stream Analytics resource, activate its managed identity and reconfigure the input for the Event Hub to use managed identity. [Give the managed identity](https://docs.microsoft.com/en-us/azure/stream-analytics/event-hubs-managed-identity) the role to access the Event Hub. This requires permissions to change resource roles.
- If managed identity cannot be configured, but restriction IP address is wanted, use the [Azure IP range](https://www.microsoft.com/en-us/download/details.aspx?id=56519) to restrict IP ranges to Azure cloud resources in your region.
- Note, if the setting is off, Stream Analytics will have a failed message in JSON like `The streaming job failed: Stream Analytics job has validation errors: Access to EventHub sb://myeventhub.servicebus.windows.net/sbeventhub is not authorized.... status-code: 401`
- After reconfiguring the input in Stream Analytics, test the connection.

### Azure Function

#### Access with Key

Use a [Function access key](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=csharp#authorization-keys) to restrict access to the function and disable anonymous access. The key requires more set up then anonymous access in the code, but prevents access if it is exposed to the internet.

#### Network firewall

As of May 2021, the recommended way to secure the Stream Analytics to Azure Function is to use a [Stream Analytics Cluster and private endpoint](https://docs.microsoft.com/en-us/azure/stream-analytics/private-endpoints). This solution restricts traffic to the function only from the Stream analytics.

If that solution is not possible, for example a cluster is expensive, it is possible to restrict traffic to the function using access restrictions and restrict traffic to the Azure cloud resources in your region by using the [Azure IP range](https://www.microsoft.com/en-us/download/details.aspx?id=56519). This reduces the IPs that could call the function, though the function still can be called to other IP ranges in Azure.
