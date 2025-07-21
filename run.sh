#!/bin/bash

# Run .NET commands using the virtual environment

set -e

# Set up the same environment variables as setup.sh
VENV_DIR=".dotnet-env"
export DOTNET_CLI_HOME="$(pwd)/$VENV_DIR"
export NUGET_PACKAGES="$(pwd)/$VENV_DIR/packages"
export DOTNET_TOOLS_PATH="$(pwd)/$VENV_DIR/tools"

# Check if virtual environment exists
if [ ! -d "$VENV_DIR" ]; then
    echo "❌ Virtual environment not found. Please run setup.sh first."
    exit 1
fi

echo "Using virtual environment: $VENV_DIR"

# Run the three commands
echo "Running dotnet restore..."
dotnet restore --packages "$NUGET_PACKAGES"

echo "Running dotnet build..."
dotnet build --no-restore

echo "Running dotnet test --list-tests..."
dotnet test --list-tests --no-build

echo "Running dotnet tests with verbosity"
dotnet test -v normal

echo "✅ All commands completed successfully!"
