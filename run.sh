#!/bin/bash

# Run .NET commands using the virtual environment

set -e

# Set up environment variables to match setup.sh
VENV_DIR=".dotnet-env"
export DOTNET_CLI_HOME="$(pwd)/$VENV_DIR"
export NUGET_PACKAGES="$(pwd)/$VENV_DIR/packages"
export DOTNET_TOOLS_PATH="$(pwd)/$VENV_DIR/tools"
export PATH="$(pwd)/$VENV_DIR/dotnet:$PATH"

# Check if virtual environment exists
if [ ! -d "$VENV_DIR/dotnet" ]; then
    echo "❌ .NET SDK not found in virtual environment. Please run setup.sh first."
    exit 1
fi

echo "Using virtual environment: $VENV_DIR"

# Run the commands
echo "Running dotnet restore..."
dotnet restore --packages "$NUGET_PACKAGES"

echo "Running dotnet build..."
dotnet build --no-restore

echo "Running dotnet test --list-tests..."
dotnet test --list-tests --no-build

echo "Running dotnet test with verbosity..."
dotnet test -v normal

echo "✅ All commands completed successfully!"
