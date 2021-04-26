[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/justintungonline/azure-sa-function-tests)

# About
The repository is for testing Azure Stream Analytics working with Azure Functions and following steps in this [Microsoft Tutorial: Run Azure Functions from Azure Stream Analytics jobs](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-with-azure-functions)

# Troubleshooting Tip
Tips and workarounds I had to use to complete the tutorial

## Create a Stream Analytics job with Event Hubs as input

[Tutorial: Analyze fraudulent call data with Stream Analytics and visualize results in Power BI dashboard](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-real-time-fraud-detection)

- Stopped at "Configure job output section describing connecting to PowerBI to visualize data". Can continue tutorial learn about PowerBI integration.
- When configuring the Stream Analytics SA input, select connection string and then the EH policy name you created.

## Create an Azure Cache for Redis instance

[Quickstart: Use Azure Cache for Redis in .NET Framework](https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-dotnet-how-to-use-azure-redis-cache#create-a-cache)

1. During creation, use a public endpoint to avoid costs with VNets and private endpoints.
2. Used V4
3. In my tutorial, redis cache took 20+ minutes to create

## Create a function in Azure Functions that can write data to Azure Cache for Redis

- Succeeded in creating function using [CLI tutorial](https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Ccurl) using the Gitpod development environment in this repository. When I tried the local tutorial, I ran into problems building the function and libraries.

[Reference of files generated for the Function App](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp#generated-project-files)

Helpful commands
```sh
# Set subscription
az account set --subscription <subscription>

# Create Azure Function app with existing resource group and storage account I created in advance of creating the function, then publish
az functionapp create --resource-group hsc-dev-46-cc-0042-e0042-sandbox --consumption-plan-location canadacentral --runtime dotnet --functions-version 3 --name sandboxfunction1 --storage-account sandboxstorageaccountehs
func azure functionapp publish sandboxfunction1
```

Things I tried during the create a function app tutorial, but did not work for me
- Manually create function in Azure portal and code only using ASE. Create new function (with new SA, App insights), go to "Function", Add HTTP trigger function, paste the code from the tutorial. Access the App Service Editor (ASE) via the function under "Development Tools" on the left side of the function configurations in the Azure Portal. Follow the tutorial on project.json, run.csx, and host.json. In the ASE, Press the run button to test the function. A page should come up indicating the function is running.

## Update the Stream Analytics job with the function as output <--- I stopped at this step
