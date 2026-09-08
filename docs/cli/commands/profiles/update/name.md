# Profile Update Name

[🏠 Home](../../../../../README.md) / [CLI Documentation](../../../../../src/WhoAmI.CLI/README.md) / [Usage](../../../usage.md) / **Profile Update Name**

---

## Description

Updates the name of a profile.

---

## Synopsis

```shell
whoami-cli profile update name [options]
```

---

## Options

| Options      | Alias | Required | Min Length | Max Length |  Type   | Domain Type                                                                      | Description            |
|:-------------|:-----:|:--------:|:----------:|:----------:|:-------:|:---------------------------------------------------------------------------------|:-----------------------|
| --id         |  -i   |   Yes    |            |            |  guid   |                                                                                  | Profile identifier.    |
| --first-name |  -f   |   Yes    |     2      |    100     |  text   | [FirstName](../../../../../src/WhoAmI.Domain/Profiles/ValueObjects/FirstName.cs) | Profile first name.    |
| --last-name  |  -l   |   Yes    |     2      |    100     |  text   | [LastName](../../../../../src/WhoAmI.Domain/Profiles/ValueObjects/LastName.cs)   | Profile last name.     |
| --help       |  -h   |    No    |            |            | boolean |                                                                                  | Displays command help. |

---

## Validation

Input validation is performed by the application layer before the command is executed.

**Validator**:
- [UpdateFullNameCommandValidator](../../../../../src/WhoAmI.Application/Features/Profiles/UpdateFullName/UpdateFullNameCommandValidator.cs)

---

## Examples

### Minimal

Update the name of a profile.

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

