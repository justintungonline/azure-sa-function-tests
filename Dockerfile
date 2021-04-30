FROM justintungonline/azure-cli-function-core-tools-dotnet:latest

# Copy files, run application
COPY --chown=gitpod:gitpod LocalFunctionProj /workspace/azure-sa-function-tests/LocalFunctionProj/
RUN az --version && func --version && dotnet --list-sdks
WORKDIR /workspace/azure-sa-function-tests/LocalFunctionProj/
EXPOSE 7071/tcp

CMD ["func", "start", "--csharp"]
