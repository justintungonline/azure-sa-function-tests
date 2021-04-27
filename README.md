[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/justintungonline/azure-sa-function-tests)

# About
The repository is for testing Azure Stream Analytics working with Azure Functions and following steps in this [Microsoft Tutorial: Run Azure Functions from Azure Stream Analytics jobs](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-with-azure-functions)

# Tips and Workarounds Used to Complete the Tutorial

List of things I had to choose in the tutorial where the tutorial doesn't specific exact steps or options to follow.

## Create a Stream Analytics job with Event Hubs as input

[Tutorial: Analyze fraudulent call data with Stream Analytics and visualize results in Power BI dashboard](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-real-time-fraud-detection)

- Stopped at "Configure job output section describing connecting to PowerBI to visualize data". Can continue tutorial learn about PowerBI integration.
- When configuring the Stream Analytics input, select connection string and then the Azure Event Hub policy name you created.

## Create an Azure Cache for Redis instance

[Quickstart: Use Azure Cache for Redis in .NET Framework](https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-dotnet-how-to-use-azure-redis-cache#create-a-cache)

1. During creation, use a public endpoint to avoid costs with VNets and private endpoints.
2. Used V4
3. In my tutorial, redis cache took 20+ minutes to create

## Create a function in Azure Functions that can write data to Azure Cache for Redis

- Created function using the command line interface (CLI) tutorial called [Quickstart: Create a C# function in Azure from the command line](https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Ccurl) using the Gitpod development environment in this repository. [Launch the online development environment](https://gitpod.io/#https://github.com/justintungonline/azure-sa-function-tests)
- The Gitpod environment is this repository has all the prerequisites for the CLI tutorial (dotnetcore, Azure CLI, Azure Function Tools Core ) preinstalled in the workspace's Ubuntu image. No local installs are required. The image has been tested with the steps in the tutorial to build the function and deploy it to Azure.

[Reference of files generated for the Function App](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp#generated-project-files)

Helpful commands
```sh
# Set subscription
az account set --subscription <subscription>

# Create Azure Function app with existing resource group and storage account I created in advance of creating the function, then publish
az functionapp create --resource-group <my-resource-group-name> --consumption-plan-location canadacentral --runtime dotnet --functions-version 3 --name sandboxfunction1 --storage-account sandboxstorageaccount
func azure functionapp publish sandboxfunction1
```

When I tried the tutorial locally in VS Code and using the Azure cloud shell as another options there were build error when building the function despite having the prerequisites.

## Update the Stream Analytics job with the function as output <--- I stopped at this step
