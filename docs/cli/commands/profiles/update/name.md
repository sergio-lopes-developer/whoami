# Profile Update Name

[🏠 Home](../../../../../README.md) / [CLI](../../../../../src/WhoAmI.CLI/README.md) / [Usage](../../../usage.md) / **Profile Update Name**

---

## Description

Updates a profile's name.

---

## Synopsis

```shell
whoami-cli profile update name [options]
```

---

## Options

| Option       | Alias | Required | Min Length | Max Length |  Type   | Domain Value Object                                                              | Description            |
|:-------------|:-----:|:--------:|:----------:|:----------:|:-------:|:---------------------------------------------------------------------------------|:-----------------------|
| --id         |  -i   |   Yes    |            |            |  GUID   |                                                                                  | Profile identifier.    |
| --first-name |  -f   |   Yes    |     2      |    100     |  Text   | [FirstName](../../../../../src/WhoAmI.Domain/Profiles/ValueObjects/FirstName.cs) | New first name.        |
| --last-name  |  -l   |   Yes    |     2      |    100     |  Text   | [LastName](../../../../../src/WhoAmI.Domain/Profiles/ValueObjects/LastName.cs)   | New last name.         |
| --help       |  -h   |    No    |            |            | Boolean |                                                                                  | Displays command help. |

---

## Validation

Input validation is performed by the Application layer before the command handler is executed.

**Validator**:
- [UpdateFullNameCommandValidator](../../../../../src/WhoAmI.Application/Features/Profiles/UpdateFullName/UpdateFullNameCommandValidator.cs)

---

## Examples

### Basic Example

Update the name.

```shell
whoami-cli profile update name \
    -i b30545b1-b92b-4040-bbca-c733deb8b1c2 \
    -f John \
    -l Smith
```

Expected output:

```text
Name successfully updated.
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
| [Profile Update Email](./email.md) | [Profile Update Social Links](./social-links.md) |

</div>

