#!/bin/bash
set -e
dotnet restore
dotnet publish -c Release -o release
