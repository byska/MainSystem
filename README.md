# Flight Roster Management System

A comprehensive .NET 8 application for managing flight crew and passenger assignments with automated roster generation, seat allocation, and multiple testing strategies.

## Quick Start

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server (for database)

### Initial Setup

1. **Navigate to the project root** (where `MainSystem.sln` is located):
   ```bash
   cd <root_folder>
   ```

2. **Set up virtual environment and install dependencies**:
   ```bash
   chmod +x setup.sh
   ./setup.sh
   ```

3. **Run the core commands anytime after setup**:
   ```bash
   chmod +x run.sh
   ./run.sh
   ```

### Cleanup Script

- **`cleanup.sh`** - Complete cleanup including virtual environment
  ```bash
  chmod +x cleanup.sh
  ./cleanup.sh
  ```
  This script removes:
  - Build artifacts (`bin/`, `obj/`)
  - Virtual environment (`.dotnet-env/`)
  - IDE files (`.vs/`, `.idea/`)
  - Test results and logs
  - User-specific files
  - NuGet caches

## Virtual Environment

The project uses an isolated .NET environment stored in `.dotnet-env/` which contains:
- **NuGet Packages**: All project dependencies isolated from global cache
- **Tools**: Project-specific .NET tools
- **Configuration**: Isolated CLI settings

### Key Dependencies Managed in Virtual Environment:
- **Microsoft.Extensions.DependencyInjection** v9.0.6
- **Microsoft.NET.Test.Sdk** v17.8.0
- **xunit** v2.4.2, **xunit.runner.visualstudio** v2.4.5
- **FluentAssertions** v6.12.0
- **Moq** v4.20.69
- **Microsoft.AspNetCore.Mvc.Testing** v8.0.0
- **Microsoft.EntityFrameworkCore.InMemory** v8.0.0
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** v8.0.18
- **Microsoft.AspNetCore.Authentication.JwtBearer** v8.0.18
- **Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation** v8.0.18
- **Swashbuckle.AspNetCore** v6.6.2
- **Microsoft.EntityFrameworkCore.Design** v8.0.18
- **MediatR** v13.0.0

## Typical Workflow

1. **Clean start** (if needed):
   ```bash
   ./cleanup.sh
   ```

2. **Set up environment**:
   ```bash
   ./setup.sh
   ```

3. **Run commands** (can be repeated):
   ```bash
   ./run.sh
   ```

## Project Structure

```
MainSystem/
├── MainSystem.sln                    # Solution file
├── MainSystem.Api/                   # Web API layer
├── MainSystem.Application/           # Application services & use cases
├── MainSystem.Domain/                # Domain entities & business logic
├── MainSystem.Infrastructure/        # Data access & external services
├── MainSystem.Tests/                 # Comprehensive test suite
│   ├── Unit/                        # Unit tests
│   ├── Integration/                 # API & database integration tests
│   ├── BlackBox/                    # Input validation tests
│   ├── Acceptance/                  # Feature-based acceptance tests
│   ├── Security/                    # Authentication & authorization tests
│   └── Performance/                 # Load & stress tests
├── .dotnet-env/                     # Virtual environment (created by setup.sh)
├── setup.sh                        # Setup script with virtual environment
├── run.sh                          # Run core commands script
├── cleanup.sh                      # Cleanup script (includes venv)
└── README.md                       # This file
```

## Testing Strategy

The project includes comprehensive testing at multiple levels:

### Test Types
- **Unit Tests** - Method/class level testing
- **Integration Tests** - API & database interaction testing
- **Black Box Tests** - Input/output validation
- **Acceptance Tests** - User story-driven feature testing
- **Security Tests** - Authentication & authorization
- **Performance Tests** - Response time & throughput
- **Load/Stress Tests** - System behavior under load

### Running Tests

```bash
# Use the run.sh script for basic test discovery
./run.sh

# Or run tests manually in the virtual environment:
export DOTNET_CLI_HOME="$(pwd)/.dotnet-env"
export NUGET_PACKAGES="$(pwd)/.dotnet-env/packages"

# List all available tests
dotnet test --list-tests

# Run all tests
dotnet test -v normal

# Run specific test categories
dotnet test --filter "Unit" -v normal
dotnet test --filter "Integration" -v normal
dotnet test --filter "SimpleTests" -v normal

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## Core Features

- **Flight Roster Generation** - Automatic crew and passenger assignment
- **Seat Assignment Algorithms** - Multiple strategies (Greedy, Group-aware, Random)
- **Crew Management** - Pilot and cabin crew validation with business rules
- **Multi-format Export** - JSON export with plane view visualization
- **Role-based Security** - Admin/Viewer access control
- **External API Integration** - Flight, crew, and passenger data sources

## Troubleshooting

### If setup fails:
1. Make sure you're in the project root (where `MainSystem.sln` is)
2. Run cleanup: `./cleanup.sh`
3. Try setup again: `./setup.sh`

### If `run.sh` fails:
1. Ensure virtual environment exists: `ls -la .dotnet-env`
2. If missing, run setup: `./setup.sh`
3. Check for .NET 8 compatibility

### Environment Issues:
- **Missing .NET 8**: The setup script will attempt to install .NET 8 SDK if needed
- **Permission Issues**: Use `sudo` if file permission errors occur during cleanup
- **Corrupted Environment**: Run `./cleanup.sh` then `./setup.sh` for fresh start

### Manual Commands (if scripts fail):
```bash
# Manual restore with virtual environment
export NUGET_PACKAGES="$(pwd)/.dotnet-env/packages"
dotnet restore --packages "$NUGET_PACKAGES"
dotnet build --no-restore
dotnet test --list-tests --no-build
```

## Notes

- Always run scripts from the **solution root** directory
- The virtual environment (`.dotnet-env/`) is excluded from Git
- Use `cleanup.sh` regularly to maintain a clean environment  
- The `.gitignore` file prevents build artifacts from being committed
- Integration tests require the API to be running
- Performance tests are optional and can be disabled if causing issues