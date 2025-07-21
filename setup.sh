#!/bin/bash

# Simple .NET 8 setup script with virtual environment

set -e

echo "Setting up .NET 8 environment with all packages..."

# Create a virtual environment directory
VENV_DIR=".dotnet-env"

if [ ! -d "$VENV_DIR" ]; then
    mkdir -p "$VENV_DIR"
    echo "Created virtual environment directory: $VENV_DIR"
fi

# Set environment variables for isolated .NET environment
export DOTNET_CLI_HOME="$(pwd)/$VENV_DIR"
export NUGET_PACKAGES="$(pwd)/$VENV_DIR/packages"
export DOTNET_TOOLS_PATH="$(pwd)/$VENV_DIR/tools"
export PATH="$(pwd)/$VENV_DIR/dotnet:$PATH"

echo "Using isolated environment at: $VENV_DIR"
echo "NuGet packages will be downloaded to: $NUGET_PACKAGES"

# Always install .NET 8 SDK (includes runtime)
DOTNET_VERSION="8.0.404"
echo "Installing .NET 8 SDK ($DOTNET_VERSION)..."
curl -sSL https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh | bash -s -- --version "$DOTNET_VERSION" --install-dir "$VENV_DIR/dotnet"

# Clear any existing packages to ensure clean install
if [ -d "$NUGET_PACKAGES" ]; then
    echo "Clearing existing packages..."
    rm -rf "$NUGET_PACKAGES"
fi

# Run the three required commands
echo "Running dotnet restore (downloading all packages to venv)..."
dotnet restore --packages "$NUGET_PACKAGES"

echo "Running dotnet build..."
dotnet build --no-restore

echo "Running dotnet test --list-tests..."
dotnet test --list-tests --no-build

echo ""
echo "✅ Setup complete with .NET $DOTNET_VERSION!"
echo "📦 Packages location: $NUGET_PACKAGES"
echo "🔧 Environment: $VENV_DIR"
