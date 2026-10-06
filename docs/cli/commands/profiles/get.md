# Profile Get

[🏠 Home](../../../../README.md) / [CLI](../../../../src/WhoAmI.CLI/README.md) / [Usage](../../usage.md) / **Profile Get**

---

## Description

Displays a profile identified by its email address.

---

## Synopsis

```shell
whoami-cli profile get [options]
```

---

## Options

| Option          | Alias | Required |  Type   | Domain Value Object                                                   | Description                                                                     |
|:----------------|:-----:|:--------:|:-------:|:----------------------------------------------------------------------|:--------------------------------------------------------------------------------|
| --email         |  -e   |   Yes    |  Email  | [Email](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Email.cs) | Profile email address.                                                          |
| --hide-email    |  -m   |    No    | Boolean |                                                                       | Hides the profile email address from the output.                                |
| --hide-github   |  -g   |    No    | Boolean |                                                                       | Hides the profile GitHub URL from the output.                                   |
| --hide-linkedin |  -n   |    No    | Boolean |                                                                       | Hides the profile LinkedIn URL from the output.                                 |
| --verbose       |  -v   |    No    | Boolean |                                                                       | Displays additional information, including the profile ID and audit timestamps. |
| --help          |  -h   |    No    | Boolean |                                                                       | Displays command help.                                                          |

---

## Validation

Input validation is performed by the Application layer before the command handler is executed.

**Validator**:
- [GetProfileByEmailQueryValidator](../../../../src/WhoAmI.Application/Features/Profiles/GetProfileByEmail/GetProfileByEmailQueryValidator.cs)

---

## Examples

### Basic Example

Retrieve a profile.

```shell
whoami-cli profile get \
    -e sergio@example.com
```

Expected output:

```text
╭───────────────── Sergio Lopes ──────────────────╮
│ Email:     sergio@example.com                   │
│ GitHub:    https://github.com/username          │
│ LinkedIn:  https://www.linkedin.com/in/username │
╰─────────────────────────────────────────────────╯
```

### Hide Email Address

Hide the email address from the output.

```shell
whoami-cli profile get \
    -e sergio@example.com \
    --hide-email
```

Expected output:

```text
╭───────────────── Sergio Lopes ──────────────────╮
│ GitHub:    https://github.com/username          │
│ LinkedIn:  https://www.linkedin.com/in/username │
╰─────────────────────────────────────────────────╯
```

### Hide GitHub URL

Hide the GitHub URL from the output.

```shell
whoami-cli profile get \
    -e sergio@example.com \
    --hide-github
```

Expected output:

```text
╭───────────────── Sergio Lopes ──────────────────╮
│ Email:     sergio@example.com                   │
│ LinkedIn:  https://www.linkedin.com/in/username │
╰─────────────────────────────────────────────────╯
```

### Hide LinkedIn URL

Hide the LinkedIn URL from the output.

```shell
whoami-cli profile get \
    -e sergio@example.com \
    --hide-linkedin
```

Expected output:

```text
╭───────────────── Sergio Lopes ──────────────────╮
│ Email:     sergio@example.com                   │
│ GitHub:    https://github.com/username          │
╰─────────────────────────────────────────────────╯
```

### Hide All Optional Information

When all optional information is hidden, the command displays a warning message.

```shell
whoami-cli profile get \
    -e sergio@example.com \
    --hide-email \
    --hide-github \
    --hide-linkedin
```

Expected output:

```text
All profile information is hidden.
Use --verbose to display additional information.
```

### Display Verbose Output

Display additional profile information, including the ID and audit timestamps.

```shell
whoami-cli profile get \
    -e sergio@example.com \
    --verbose
```

Expected output:

```text
╭────────────────── Sergio Lopes ───────────────────╮
│ ID:          b30545b1-b92b-4040-bbca-c733deb8b1c2 │
│                                                   │
│ Email:       sergio@example.com                   │
│ GitHub:      https://github.com/username          │
│ LinkedIn:    https://www.linkedin.com/in/username │
│                                                   │
│ Created at:  2026-09-22 17:48:31 -03:00           │
│ Updated at:  Never                                │
╰───────────────────────────────────────────────────╯
```

---

## Exit Codes

| Code | Description |
|:----:|:------------|
|  0   | Success     |
|  1   | Error       |

---

<div align="center">

| &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
| :----------------------------------------------------: | :------------------------------------------------------------------------: |
| [Profile Create](./create.md) | [Profile List](./list.md) |

</div>

