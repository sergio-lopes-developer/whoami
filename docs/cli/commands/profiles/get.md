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

| Option          | Alias | Required |  Type   | Domain Value Object                                                   | Description                                                      |
|:----------------|:-----:|:--------:|:-------:|:----------------------------------------------------------------------|:-----------------------------------------------------------------|
| --email         |  -e   |   Yes    |  Email  | [Email](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Email.cs) | Profile email address.                                           |
| --hide-email    |  -m   |    No    | Boolean |                                                                       | Hides the profile email address from the output.                 |
| --hide-linkedin |  -n   |    No    | Boolean |                                                                       | Hides the profile LinkedIn URL from the output.                  |
| --verbose       |  -v   |    No    | Boolean |                                                                       | Displays additional information, such as the profile identifier. |
| --help          |  -h   |    No    | Boolean |                                                                       | Displays command help.                                           |

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
╭─────────────────── PROFILE ────────────────────╮
│ Name      Sérgio Lopes                         │
│ Email     sergio@example.com                   │
│ GitHub    https://github.com/username          │
│ LinkedIn  https://www.linkedin.com/in/username │
╰────────────────────────────────────────────────╯
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
╭─────────────────── PROFILE ────────────────────╮
│ Name      Sérgio Lopes                         │
│ GitHub    https://github.com/username          │
│ LinkedIn  https://www.linkedin.com/in/username │
╰────────────────────────────────────────────────╯
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
╭─────────────────── PROFILE ────────────────────╮
│ Name      Sérgio Lopes                         │
│ Email     sergio@example.com                   │
│ GitHub    https://github.com/username          │
╰────────────────────────────────────────────────╯
```

### Display Verbose Output

Display additional information.

```shell
whoami-cli profile get \
    -e sergio@example.com \
    --verbose
```

Expected output:

```text
╭─────────────────── PROFILE ────────────────────╮
│ Id        b30545b1-b92b-4040-bbca-c733deb8b1c2 │
│ Name      Sérgio Lopes                         │
│ Email     sergio@example.com                   │
│ GitHub    https://github.com/username          │
│ LinkedIn  https://www.linkedin.com/in/username │
╰────────────────────────────────────────────────╯
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

