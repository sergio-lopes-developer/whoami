# Quick Start

[🏠 Home](../../README.md) / [CLI](../../src/WhoAmI.CLI/README.md) / **Quick Start**

---

## Introduction

This guide explains how to set up and run the **WhoAmI CLI** for the first time.

---

## Prerequisites

Before getting started, ensure the following tools are installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download) — Download and installation instructions for Linux, macOS, and Windows.
- [Git](https://git-scm.com/downloads) — Download and installation instructions for Linux, macOS, and Windows.

Verify the installation by running:

```bash
dotnet --version
git --version
```

Each command should print the installed version.

The repository includes a [global.json](../../global.json) file that defines the minimum supported .NET SDK version for the project.

---

## 1. Clone the Repository

Open a terminal and clone the repository:

```shell
git clone https://github.com/sergio-lopes-developer/whoami.git
cd whoami
```

---

## 2. Build the Solution

Restore the project dependencies:

```shell
dotnet restore
```

Build the solution:

```shell
dotnet build
```

---

## 3. Verify Installation

Run the following command:

```shell
dotnet run --project src/WhoAmI.CLI -- profile list
```

If the database does not exist, the CLI automatically creates it and applies any pending migrations using the default configuration.

If the database is empty, you should see:

```text
No profiles were found.
```

If the command completes successfully, your **WhoAmI CLI** setup is complete.

---

## Next Steps

Now that the CLI is running, continue with:

- [Usage](./usage.md) — Learn the available commands.
- [Configuration](./configuration.md) — Customize the database connection and application settings.

---

<div align="center">

| &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
|:------------------------------------------------------:|:--------------------------------------------------------------------------:|
| [CLI](../../src/WhoAmI.CLI/README.md) | [Configuration](./configuration.md) |

</div>
