# FatCat.Worker Coding Standards

This file defines the coding standards for the FatCat.Worker codebase.
All code you generate — in any context — must follow the rules for the relevant language below.
The goal is that AI-generated code is indistinguishable from code written by a senior member of this team.

## ⚠️ READ THIS FIRST — Before Any Work

**Before doing anything else in this repository — reading, planning, or writing code — read [`README.md`](README.md) in full.**
It is the single source of truth for what this product is and how it is built. Do not skip it, even for a "small" change.

The README tells you the things you must know before you touch a line of code:
- **Product:** FatCat.Worker — a lightweight NuGet library (`FatCat.Worker`) for running recurring worker tasks in .NET. A consumer implements `IWorker` (an `Interval` plus `DoWork()`); `WorkerRunner` discovers every `IWorker` in the loaded assemblies by reflection, resolves it from the FatCat `SystemScope` (Autofac) container, and runs it on a timer. Variants run a worker at a specific time (`IRunAtSpecificTimeWorker`), a limited number of times (`IRunLimitedNumberWorker`), or only when added at runtime (`IDynamicWorker` via `AddDynamicWorker`).
- **Tech stack:** **.NET 11 / C#** (`LangVersion` latest) class library built on the **FatCat.Toolkit** (injection, reflection, logging). Tests use **xUnit**, **FakeItEasy**, and **FluentAssertions**. There is no UI, web server, or database in this repo.
- **Architecture:** `Worker` is the shipped library; `Tests.Worker` holds its unit tests; `OneOff` is a scratch console app for trying workers by hand and is not shipped.
- **Standing rules:** this is a published package — changes to a public interface (`IWorker`, `IWorkerRunner`, and their variants) are breaking changes for consumers, and the version is stepped in `VersionPrefix` in `Worker.csproj`.

If anything below in the coding standards appears to conflict with the README, the README describes *what* the system is; these rules describe *how* to write code for it — follow both. Where the README and the actual code disagree, **the code is authoritative** — trust the code and flag the stale README.

## Repository Layout

| Path | Project | Purpose |
|---|---|---|
| `src/Worker` | `FatCat.Worker` | The NuGet library: `IWorker` and its variants, `WorkerRunner`, `TimerWorker`, `WorkerModule`, timer wrappers |
| `src/Tests.Worker` | `Tests.FatCat.Worker` | xUnit tests for `FatCat.Worker` (`WorkerRunnerSpecs`, `TimerWorkerItemSpecs`) |
| `src/OneOff` | `OneOff` | Console app for manually exercising workers; not shipped |

All production namespaces start with `FatCat.Worker.*`; test namespaces start with `Tests.FatCat.Worker.*`.

Everything must build and every test must pass before work is considered done:
```bash
dotnet build src/Worker.slnx
dotnet test src/Worker.slnx
```

---

## C# Rules

Apply these rules to all C# code. Do not apply them to TypeScript, PowerShell, or any other language.

@.claude/rules/csharp/naming-and-structure.md
@.claude/rules/csharp/types-and-di.md
@.claude/rules/csharp/toolchain.md
@.claude/rules/csharp/async.md
@.claude/rules/csharp/errors-and-logging.md
@.claude/rules/csharp/testing.md
@.claude/rules/csharp/not-allowed.md

## PowerShell Rules

Apply these rules to all PowerShell scripts. Do not apply them to C# or any other language.

@.claude/rules/powershell/powershell.md
