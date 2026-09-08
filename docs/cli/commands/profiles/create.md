# Profile Create

[🏠 Home](../../../../README.md) / [CLI](../../../../src/WhoAmI.CLI/README.md) / [Usage](../../usage.md) / **Profile Create**

---

## Description

Creates a new profile.

---

## Synopsis

```shell
whoami-cli profile create [options]
```

---

## Options

| Options      | Alias | Required | Min Length | Max Length |  Type   | Domain Type                                                                   | Description            |
|:-------------|:-----:|:--------:|:----------:|:----------:|:-------:|-------------------------------------------------------------------------------|:-----------------------|
| --first-name |  -f   |   Yes    |     2      |    100     |  text   | [FirstName](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/FirstName.cs) | Profile first name.    |
| --last-name  |  -l   |   Yes    |     2      |    100     |  text   | [LastName](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/LastName.cs)   | Profile last name.     |
| --email      |  -e   |   Yes    |            |            |  email  | [Email](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Email.cs)         | Profile email address. |
| --linkedin   |  -n   |   Yes    |            |    255     |   URL   | [Url](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs)             | Profile LinkedIn URL.  |
| --github     |  -g   |   Yes    |            |    255     |   URL   | [Url](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs)             | Profile GitHub URL.    |
| --help       |  -h   |    No    |            |            | boolean |                                                                               | Displays command help. |

---

## Validation

Input validation is performed by the application layer before the command is executed.

**Validator**:
- [CreateProfileCommandValidator](../../../../src/WhoAmI.Application/Features/Profiles/CreateProfile/CreateProfileCommandValidator.cs)

---

## Examples

### Minimal

Create a profile.

```shell
whoami-cli profile create \
    -f Sergio \
    -l Lopes \
    -e sergio@example.com \
    -n https://linkedin.com/in/username \
    -g https://github.com/username
```

Expected output:

```text
Profile successfully created.
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
| [Usage](../../usage.md) | [Profile Get](./get.md) |

</div>

