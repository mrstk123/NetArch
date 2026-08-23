# Understanding Template Variables (`dotnet new` vs. IDE)

When working on a custom `dotnet new` template, it's crucial to understand that your code is analyzed by two completely different engines at different times:
1. **The IDE (Visual Studio / Rider / MSBuild)** – when you are actively writing the template code.
2. **The `dotnet new` Engine** – when your user is scaffolding their brand new project.

Because these two engines function differently, we use a few tricks to make sure the template authoring experience is smooth and error-free.

---

## 1. The `dotnet new` Templating Engine
When a user runs `dotnet new net-arch --architecture ntier`, the template engine acts as a **text preprocessor**.

### How it behaves:
- It evaluates the mathematical truth of variables defined in `template.json` (e.g., calculates that `IsNTier = true`).
- It scans `.cs` files for standard C# `#if (IsNTier)` blocks. 
- It scans `.xml` and `.csproj` files looking for matching template comments like `<!--#if (IsNTier)-->`.
- **Execution:** It physically deletes the blocks of text/code where the `#if` statement evaluates to `false` and leaves the `true` code intact. It finally deletes the `#if` comment lines themselves to leave a pristine generated file.
- **Dependency on `csproj`:** The engine **does not care** if `<DefineConstants>` exist in your `.csproj` files; it relies entirely on its `template.json` configuration context.

---

## 2. The IDE & MSBuild (Authoring the Template)
When you open `NetArch.Template.sln` to actively develop the template, your IDE's compiler (Roslyn) analyzes the raw source code. It has no idea that `template.json` exists.

### How it behaves:
- It processes standard C# `#if (IsNTier)` blocks natively.
- **The Problem:** Since no `DefineConstants` are ever defined while authoring, every template symbol evaluates to `false`. This grays out your conditional code, disables `using` statements, and hides IntelliSense for code paths that only appear in some variants.
- **The Reality:** Conditional code is *never* compiled while authoring the template. It is only compiled after a user scaffolds a project and builds it.

### What this means in practice:
1. **Keep every conditional block self-contained.** A `#if` block must compile on its own: its `using` directives, base types, and referenced symbols must all be guarded by the same (or broader) conditions. An unguarded `using` pointing at a file excluded by `template.json` will break that variant's build.
2. **Validate by scaffolding, not by building the source solution.** Building `NetArch.Template.sln` proves almost nothing because most conditionals compile to empty files. The real gate is generating and building each variant:
   ```bash
   dotnet new net-arch -n TestApp -o /tmp/test --architecture Clean --orm Dapper
   dotnet build /tmp/test/TestApp.sln
   ```
   Run this matrix over `architecture × orm` (`Clean/NTier × EFCore/Dapper/Hybrid`) whenever you touch conditional code.
3. **Watch out for name collisions in generated namespaces.** The user's project name becomes the root namespace. Code like `EF.Property<T>(...)` breaks if a project is named e.g. `Contoso.EF`, because the namespace segment `EF` shadows the `Microsoft.EntityFrameworkCore.EF` class. Prefer strongly-typed expressions over `EF.Property`, or fully qualify such helpers.
