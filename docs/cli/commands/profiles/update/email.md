# Profile Update Email

[🏠 Home](../../../../../README.md) / [CLI](../../../../../src/WhoAmI.CLI/README.md) / [Usage](../../../usage.md) / **Profile Update Email**

---

## Description

Updates the email address of a profile.

---

## Synopsis

```shell
whoami-cli profile update email [options]
```

---

## Options

| Options | Alias | Required |  Type   | Domain Type                                                           | Description                |
|:--------|:-----:|:--------:|:-------:|:----------------------------------------------------------------------|:---------------------------|
| --id    |  -i   |   Yes    |  guid   |                                                                       | Profile identifier.        |
| --email |  -e   |   Yes    |  email  | [Email](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Email.cs) | New profile email address. |
| --help  |  -h   |    No    | boolean |                                                                       | Displays command help.     |

---

## Validation

Input validation is performed by the application layer before the command is executed.

**Validator**:
- [UpdateEmailCommandValidator](../../../../../src/WhoAmI.Application/Features/Profiles/UpdateEmail/UpdateEmailCommandValidator.cs)

---

## Examples

### Minimal

Update the email address of a profile.

```shell
whoami-cli profile update email \
    -i b30545b1-b92b-4040-bbca-c733deb8b1c2 \
    -e sergio.contact@example.com
```

Expected output:

```text
Email successfully updated.
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
| [Profile List](../list.md) | [Profile Update Name](./name.md) |

</div>

