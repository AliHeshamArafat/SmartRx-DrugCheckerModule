#!/usr/bin/env bash
set -e

# Install .NET runtime if not already installed (for runtime environment)
if [ ! -d "/tmp/dotnet" ]; then
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.100 --install-dir /tmp/dotnet --runtime aspnetcore
fi

# Set PATH to include .NET runtime
export PATH="/tmp/dotnet:$PATH"
export DOTNET_ROOT="/tmp/dotnet"

# Navigate to publish directory
cd publish

# Run the application
dotnet SmartRx-DrugChecker.dll
