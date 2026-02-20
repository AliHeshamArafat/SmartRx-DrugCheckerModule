#!/bin/bash
set -e

# Install .NET 8.0 SDK
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.100 --install-dir /tmp/dotnet
export PATH="/tmp/dotnet:$PATH"
export DOTNET_ROOT="/tmp/dotnet"

# Verify installation
dotnet --version

# Restore and publish
dotnet restore
dotnet publish -c Release -o ./publish
