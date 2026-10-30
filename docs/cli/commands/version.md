# Version

[🏠 Home](../../../README.md) / [CLI](../../../src/WhoAmI.CLI/README.md) / [Usage](../usage.md) / **Version**

---

## Description

Displays the CLI version.

---

## Synopsis

Primary command:

```shell
whoami-cli version [options]
```

Alias:

```shell
whoami-cli v [options]
```

---

## Options

| Option    | Alias | Required |  Type   | Description                                        |
|:----------|:-----:|:--------:|:-------:|:---------------------------------------------------|
| --verbose |  -v   |    No    | Boolean | Displays additional build and runtime information. |
| --help    |  -h   |    No    | Boolean | Displays command help.                             |

---

## Validation

This command does not accept user input and therefore requires no validation.

---

## Examples

### Basic Example

Display the CLI version.

```shell
whoami-cli version
```

Expected output:

```text
WhoAmI CLI v1.0.0
```

### Display Additional Version Information

Display the CLI version together with build and runtime information.

```shell
whoami-cli version --verbose
```

Expected output:

```text
WhoAmI CLI v1.0.0
──────────────────────────────────────────────────────
Commit:       a79aca149a55f19f46fa7a6f63b88cfad281bf80
Environment:  Windows 11
Architecture: X64
.NET Runtime: 10.0.14

```

> [!NOTE]
>
> The values displayed for Commit, Environment, Architecture, and .NET Runtime depend on the current build and execution environment.

### Using the Alias

Display the CLI version using the command alias.

```shell
whoami-cli v
```

Expected output:

```text
WhoAmI CLI v1.0.0
```

---

## Exit Codes

| Code | Description |
|:----:|:------------|
|  0   | Success     |
|  1   | Error       |

---

<div align="center">

|      &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp;      | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
|:----------------------------------------------------------------:|:--------------------------------------------------------------------------:|
| [Profile Update Social Links](./profiles/update/social-links.md) | [CLI](../../../src/WhoAmI.CLI/README.md) |

</div>
