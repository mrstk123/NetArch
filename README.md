# NetArch Architecture Template

Welcome to the **NetArch Architecture Template**! This template allows you to rapidly scaffold a new ASP.NET Core project tailored specifically to your architectural, database, and UI preferences.

## Installation

There are two primary ways to install the NetArch Architecture Template on your machine.

### Option 1: Install via NuGet Package (Recommended)

You can download the latest pre-compiled `.nupkg` file directly from the [GitHub Releases](https://github.com/mrstk123/NetArch/releases) page of this repository.

Once downloaded, run the following command from the directory where the `.nupkg` is saved:

```bash
dotnet new install ./NetArch.Template.<version>.nupkg
```

### Option 2: Install from Source (Git Clone)

Alternatively, you can clone the repository to your local machine and install the template directly from the source code directory:

```bash
# Clone the repository
git clone https://github.com/mrstk123/NetArch.git
cd NetArch

# Install the template
dotnet new install .
```

> **Note:** To update or reinstall an existing version of this template from source, simply append the `--force` flag: `dotnet new install . --force`

---

## Creating a New Project

Once installed, you can create a new project anywhere on your system using the `net-arch` short name.

### Basic Usage

To scaffold a project using the default options (Clean Architecture + EF Core + No UI) into a specific folder:

```bash
dotnet new net-arch -n MyNewProject -o ./MyNewProject
```

### Advanced Usage (Custom Parameters)

You can heavily customize the generated project architecture by passing parameters to the `dotnet new net-arch` command. 

Here are all the available options you can mix and match:

#### 1. Architecture (`--architecture` or `-a`)
Defines the core software pattern of the project.
* **`Clean`** *(Default)*: Generates Domain, Application, Infrastructure, and WebAPI layers. Highly decoupled architecture.
* **`NTier`**: Generates BusinessLogic, Infrastructure, and WebAPI layers. A more straightforward, traditional approach.

> Example: `dotnet new net-arch --architecture clean`

#### 2. Object-Relational Mapper / DB Strategy (`--orm` or `-o`)
Defines how your application connects and maps data to the database.
* **`EFCore`** *(Default)*: Scaffolds Entity Framework Core contexts, configurations, and repositories.
* **`Dapper`**: Scaffolds lightweight Dapper micro-ORM querying logic.
* **`Hybrid`**: Combines both approaches—using EF Core for complex writes and Dapper for high-performance reads.

> Example: `dotnet new net-arch --orm efcore`

#### 3. User Interface (`-ui` or `-u`)
Defines the front-end implementation of your project.
* **`None`** *(Default)*: Scaffolds a Back-end API only.
* **`Angular`**: Scaffolds an ASP.NET Core backend bundled with a full Angular Single Page Application (SPA).

> Example: `dotnet new net-arch --ui angular`

---

## Full Example

If you want to create a Clean Architecture application, utilizing EF Core for all database interactions, paired with an Angular front-end, you would run:

```bash
dotnet new net-arch -n MyApp -o ./MyApp --architecture clean --orm efcore --ui angular
```

## Uninstalling the Template

If you installed from source (Option 2), run:

```bash
dotnet new uninstall .
```

If you installed via NuGet package (Option 1), run:

```bash
dotnet new uninstall NetArch.Template
```

---

## Contributing & Packaging

If you are modifying the template source code and wish to build a new `.nupkg` release artifact yourself, you can run the following command from the root directory:

```bash
dotnet pack NetArch.Template.csproj -c Release
```

This generates a fresh `NetArch.Template.<version>.nupkg` file in the `bin/Release` folder, which can then be published to GitHub Releases or NuGet feeds.
