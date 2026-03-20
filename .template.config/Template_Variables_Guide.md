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
When you open `NetArch.Template.sln` to actively develop the template, your IDE’s compiler (Roslyn) analyzes the raw source code. It has no idea that `template.json` exists.

### How it behaves:
- It processes standard C# `#if (IsNTier)` blocks natively.
- **The Problem:** If the IDE doesn't know what `IsClean` or `IsEFCore` means, it treats them as `false` by default. This grays out your code, disables your interface `using` statements, and creates dozens of "Missing Namespace" compiler errors.
- **The Solution:** We manually inject `<DefineConstants>` into the `.csproj` files:
  ```xml
  <PropertyGroup Condition="'$(IsTemplateGenerated)' == ''">
    <!-- This forcefully turns ON the code paths we want the IDE to compile for us -->
    <DefineConstants>$(DefineConstants);IsEFCore;IsClean</DefineConstants>
  </PropertyGroup>
  ```

### The `IsTemplateGenerated` Sandbox Strategy
To ensure that those `<DefineConstants>` (which help our IDE) do not accidentally end up permanently injected into our user's scaffolded application, we use the `IsTemplateGenerated` trigger string:

1. **In the `.csproj`**: The code is wrapped inside `<PropertyGroup Condition="'$(IsTemplateGenerated)' == ''">`. Because no such variable exists while you author the template, it evaluates to `true`, and the IDE turns on the constants.
2. **In `template.json`**: We configure `#StripSourceCompilation` to aggressively search the entire codebase for the literal string `Condition="'$(IsTemplateGenerated)' == ''"` and text-replace it with `Condition="false"`.
3. **In the Generated User App**: The user's freshly generated `.csproj` now reads `<PropertyGroup Condition="false">`. MSBuild sees `false` and completely skips over the `<DefineConstants>`, executing a perfectly clean project generation hook!
