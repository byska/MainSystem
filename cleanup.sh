#!/bin/bash

# Deep .NET Project Cleanup Script
# This script aggressively removes ALL build artifacts and caches

echo "🚨 DEEP CLEANUP - This will remove ALL build artifacts and caches!"
echo "⚠️  This may take a moment..."

# Function to force remove directories and files
force_remove() {
    if [ -e "$1" ]; then
        echo "💥 Force removing: $1"
        rm -rf "$1" 2>/dev/null || sudo rm -rf "$1" 2>/dev/null || true
    fi
}

# Clean each project individually
for project in MainSystem.Api MainSystem.Application MainSystem.Domain MainSystem.Infrastructure MainSystem.Tests; do
    if [ -d "$project" ]; then
        echo "🧹 Deep cleaning $project..."
        force_remove "$project/bin"
        force_remove "$project/obj"
        
        # Remove all hidden build files
        find "$project" -name ".vs" -type d -exec rm -rf {} + 2>/dev/null || true
        find "$project" -name "*.user" -type f -delete 2>/dev/null || true
        find "$project" -name "*.suo" -type f -delete 2>/dev/null || true
    fi
done

# Force remove all bin/obj directories everywhere
echo "🔥 Force removing all bin/obj directories..."
find . -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} + 2>/dev/null || true

# Clean root level files
force_remove ".vs"
force_remove ".idea"
force_remove "packages"
force_remove "TestResults"

# Remove virtual environment
force_remove ".dotnet-env"

# Clean NuGet caches
echo "📦 Cleaning NuGet caches..."
dotnet nuget locals all --clear 2>/dev/null || true

# Clean MSBuild cache
echo "🔨 Cleaning MSBuild cache..."
dotnet build-server shutdown 2>/dev/null || true

# Remove all temporary files
echo "🗑️ Removing temporary files..."
find . -name "*.tmp" -type f -delete 2>/dev/null || true
find . -name "*.temp" -type f -delete 2>/dev/null || true
find . -name "*.bak" -type f -delete 2>/dev/null || true
find . -name "*.orig" -type f -delete 2>/dev/null || true
find . -name "*~" -type f -delete 2>/dev/null || true
find . -name "*.log" -type f -delete 2>/dev/null || true

# Remove macOS and Linux temp files
find . -name ".DS_Store" -type f -delete 2>/dev/null || true
find . -name "Thumbs.db" -type f -delete 2>/dev/null || true

echo ""
echo "✅ Deep cleanup completed!"
echo "🔄 Now run: ./setup.sh"
echo "🚀 Then run: ./run.sh"