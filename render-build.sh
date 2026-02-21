#!/usr/bin/env bash
set -e

# Install .NET 8.0 SDK (required for Render build environment)
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.100 --install-dir /tmp/dotnet
export PATH="/tmp/dotnet:$PATH"
export DOTNET_ROOT="/tmp/dotnet"

# Restore and publish
dotnet restore
dotnet publish -c Release -o ./publish

# Make start script executable
chmod +x render-start.sh
