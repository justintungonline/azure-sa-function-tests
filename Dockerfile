FROM justintungonline/azure-cli-function-core-tools-dotnet:latest

# Copy files, run application
COPY LocalFunctionProj /workspace/azure-sa-function-tests/LocalFunctionProj/
RUN az --version && func --version && dotnet --list-sdks && cd /workspace/azure-sa-function-tests/LocalFunctionProj/ && func start
