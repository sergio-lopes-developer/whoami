# Profile Update Email

[🏠 Home](../../../../../README.md) / [CLI](../../../../../src/WhoAmI.CLI/README.md) / [Usage](../../../usage.md) / **Profile Update Email**

---

## Description

Updates a profile's email address.

---

## Synopsis

```shell
whoami-cli profile update email [options]
```

---

## Options

| Option  | Alias | Required |  Type   | Domain Value Object                                                   | Description            |
|:--------|:-----:|:--------:|:-------:|:----------------------------------------------------------------------|:-----------------------|
| --id    |  -i   |   Yes    |  GUID   |                                                                       | Profile identifier.    |
| --email |  -e   |   Yes    |  Email  | [Email](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Email.cs) | New email address.     |
| --help  |  -h   |    No    | Boolean |                                                                       | Displays command help. |

---

## Validation

Input validation is performed by the Application layer before the command handler is executed.

**Validator**:
- [UpdateEmailCommandValidator](../../../../../src/WhoAmI.Application/Features/Profiles/UpdateEmail/UpdateEmailCommandValidator.cs)

---

## Examples

### Basic Example

Update the email address.

```shell
whoami-cli profile update email \
    -i b30545b1-b92b-4040-bbca-c733deb8b1c2 \
    -e sergio.contact@example.com
```

Expected output:

```text
Email successfully updated.
```

> [!NOTE]
>
> - Email addresses must be unique.

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

