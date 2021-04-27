FROM gitpod/workspace-dotnet-lts

# Install custom tools, runtimes, etc.
# For example "bastet", a command-line tetris clone:
# RUN brew install bastet
#
# More information: https://www.gitpod.io/docs/config-docker/

# Install Azure Functions Core Tools 
# per https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=linux%2Ccsharp%2Cbash#v2
RUN curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
RUN sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
RUN sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-$(lsb_release -cs)-prod $(lsb_release -cs) main" > /etc/apt/sources.list.d/dotnetdev.list'
RUN sudo apt-get update && sudo apt-get install azure-functions-core-tools-3

# Install Azure CLI 
# per https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-linux?pivots=apt
RUN sudo apt remove azure-cli -y && sudo apt autoremove -y
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
