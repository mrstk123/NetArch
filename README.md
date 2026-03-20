# NetArch Architecture Template

Welcome to the **NetArch Architecture Template**! This template allows you to rapidly scaffold a new ASP.NET Core project tailored specifically to your architectural, database, and UI preferences.

## Installation

To install the template on your local machine, open your terminal, navigate to the root directory of this repository, and run the following command:

```bash
dotnet new install .
```

To update or reinstall an existing version of this template, simply append the `--force` flag:

```bash
dotnet new install . --force
```

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

> Example: `dotnet new net-arch --architecture ntier`

#### 2. Object-Relational Mapper / DB Strategy (`--orm` or `-o`)
Defines how your application connects and maps data to the database.
* **`EFCore`** *(Default)*: Scaffolds Entity Framework Core contexts, configurations, and repositories.
* **`Dapper`**: Scaffolds lightweight Dapper micro-ORM querying logic.
* **`Hybrid`**: Combines both approaches—using EF Core for complex writes and Dapper for high-performance reads.

> Example: `dotnet new net-arch --orm hybrid`

#### 3. User Interface (`-ui` or `-u`)
Defines the front-end implementation of your project.
* **`None`** *(Default)*: Scaffolds a Back-end API only.
* **`Angular`**: Scaffolds an ASP.NET Core backend bundled with a full Angular Single Page Application (SPA).

> Example: `dotnet new net-arch --ui angular`

---

## Full Example

If you want to create an N-Tier application, utilizing Dapper for all database interactions, paired with an Angular front-end, you would run:

```bash
dotnet new net-arch -n MyStoreApp -o ./MyStoreApp --architecture ntier --orm dapper --ui angular
```

## Uninstalling the Template

If you ever wish to remove the template from your local dotnet tools, run:

```bash
dotnet new uninstall .
```
