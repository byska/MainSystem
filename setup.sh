#!/bin/bash

# Simple .NET setup script with virtual environment

set -e

echo "Setting up .NET environment with all packages..."

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

echo "Using isolated environment at: $VENV_DIR"
echo "NuGet packages will be downloaded to: $NUGET_PACKAGES"

# Install .NET 8 SDK if not available (for systems with only .NET 9)
if ! dotnet --list-sdks | grep -q "8\."; then
    echo "Installing .NET 8 SDK..."
    curl -sSL https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh | bash -s -- --version 8.0.404 --install-dir "$VENV_DIR/dotnet"
    export PATH="$(pwd)/$VENV_DIR/dotnet:$PATH"
fi

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
echo "Setup complete! All packages downloaded to virtual environment:"
echo "📦 Packages location: $NUGET_PACKAGES"
echo "🔧 Environment: $VENV_DIR"
echo ""
echo "Downloaded packages include:"
echo "• Microsoft.Extensions.DependencyInjection v9.0.6"
echo "• Microsoft.NET.Test.Sdk v17.8.0"
echo "• xunit v2.4.2"
echo "• FluentAssertions v6.12.0"
echo "• Moq v4.20.69"
echo "• Microsoft.AspNetCore.Mvc.Testing v8.0.0"
echo "• Microsoft.EntityFrameworkCore.InMemory v8.0.0"
echo "• Microsoft.AspNetCore.Identity.EntityFrameworkCore v8.0.18"
echo "• Microsoft.AspNetCore.Authentication.JwtBearer v8.0.18"
echo "• Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation v8.0.18"
echo "• Swashbuckle.AspNetCore v6.6.2"
echo "• Microsoft.EntityFrameworkCore.Design v8.0.18"
echo "• MediatR v13.0.0"
echo "• And all their dependencies!"