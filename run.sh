#!/bin/bash

# Run .NET commands using the virtual environment (Unit tests only)

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

echo "🧪 Using virtual environment: $VENV_DIR"
echo "🧪 Running Unit Tests Only (Database tests disabled)"

# Run the commands
echo "📦 Running dotnet restore..."
dotnet restore --packages "$NUGET_PACKAGES"

echo "🔨 Running dotnet build..."
dotnet build --no-restore

echo "📋 Listing unit tests only..."
dotnet test --list-tests --filter "FullyQualifiedName~Unit|FullyQualifiedName~BlackBox" --no-build

echo "🧪 Running unit tests with verbosity..."
dotnet test --filter "FullyQualifiedName~Unit|FullyQualifiedName~BlackBox" -v normal --no-build

echo ""
echo "✅ Unit tests completed successfully!"
echo "ℹ️  Database-dependent tests were skipped (Integration, Acceptance, Security)"
echo "📊 Use these results for your test report"