# Profile Create

[🏠 Home](../../../../README.md) / [CLI](../../../../src/WhoAmI.CLI/README.md) / [Usage](../../usage.md) / **Profile Create**

---

## Description

Creates a new profile using the supplied personal information.

---

## Synopsis

```shell
whoami-cli profile create [options]
```

---

## Options

| Option       | Alias | Required | Min Length | Max Length |  Type   | Domain Value Object                                                           | Description            |
|:-------------|:-----:|:--------:|:----------:|:----------:|:-------:|-------------------------------------------------------------------------------|:-----------------------|
| --first-name |  -f   |   Yes    |     2      |    100     |  Text   | [FirstName](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/FirstName.cs) | Profile first name.    |
| --last-name  |  -l   |   Yes    |     2      |    100     |  Text   | [LastName](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/LastName.cs)   | Profile last name.     |
| --email      |  -e   |   Yes    |            |            |  Email  | [Email](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Email.cs)         | Profile email address. |
| --linkedin   |  -n   |   Yes    |            |    255     |   URL   | [Url](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs)             | Profile LinkedIn URL.  |
| --github     |  -g   |   Yes    |            |    255     |   URL   | [Url](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs)             | Profile GitHub URL.    |
| --help       |  -h   |    No    |            |            | Boolean |                                                                               | Displays command help. |

---

## Validation

Input validation is performed by the Application layer before the command handler is executed.

**Validator**:
- [CreateProfileCommandValidator](../../../../src/WhoAmI.Application/Features/Profiles/CreateProfile/CreateProfileCommandValidator.cs)

---

## Examples

### Basic Example

Create a profile.

```shell
whoami-cli profile create \
    -f Sergio \
    -l Lopes \
    -e sergio@example.com \
    -n https://www.linkedin.com/in/username \
    -g https://github.com/username
```

Expected output:

```text
Profile successfully created.
```

> [!NOTE]
>
> - Email addresses must be unique.
> - LinkedIn and GitHub URLs must be valid absolute URLs.

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
| [Usage](../../usage.md) | [Profile Get](./get.md) |

</div>

