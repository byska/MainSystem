#!/bin/bash

# Run .NET commands using the virtual environment (Test discovery only)

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
echo "✅ .NET version: $(dotnet --version)"

echo ""
echo "🔄 Running basic operations..."

echo "📦 Running dotnet restore..."
dotnet restore --packages "$NUGET_PACKAGES"
echo "✅ Restore completed"

echo "🔨 Running dotnet build..."
dotnet build --no-restore
echo "✅ Build completed successfully"

echo ""
echo "🧪 Running Tests..."

echo "📋 Discovering tests..."
TEST_COUNT=$(dotnet test --list-tests --no-build 2>/dev/null | grep -c "    " || echo "0")
echo "✅ Found $TEST_COUNT total tests"

echo ""
echo "🎯 Running Unit Tests (safer, no external dependencies)..."
dotnet test --filter "Category=Unit" -v normal --no-build
echo "✅ Unit tests completed successfully"

echo ""
echo "📦 Running Input Validation Tests..."
dotnet test --filter "Category!=Integration&Category!=Security&FullyQualifiedName~BlackBox" -v normal --no-build
echo "✅ Input validation tests completed"

echo ""
echo "📊 Test Summary:"
echo "✅ Core functionality tests completed"
echo "⚠️  Integration tests skipped (require external APIs)"
echo "⚠️  Database tests skipped (require SQL Server)"

echo ""
echo "💡 To run all tests (including integration):"
echo "   1. Set up SQL Server database"
echo "   2. Configure external API endpoints in appsettings.json"
echo "   3. Run: dotnet test --verbosity normal"

echo ""
echo "🎉 Basic test run completed!"