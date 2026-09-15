# Profile Update Social Links

[🏠 Home](../../../../../README.md) / [CLI](../../../../../src/WhoAmI.CLI/README.md) / [Usage](../../../usage.md) / **Profile Update Social Links**

---

## Description

Updates a profile's social links.

---

## Synopsis

```shell
whoami-cli profile update social-links [options]
```

---

## Options

| Option     | Alias | Required | Max Length |  Type   | Domain Value Object                                                  | Description            |
|:-----------|:-----:|:--------:|:----------:|:-------:|:---------------------------------------------------------------------|:-----------------------|
| --id       |  -i   |   Yes    |            |  GUID   |                                                                      | Profile identifier.    |
| --linkedin |  -n   |   Yes    |    255     |   URL   | [Url](../../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs) | New LinkedIn URL.      |
| --github   |  -g   |   Yes    |    255     |   URL   | [Url](../../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs) | New GitHub URL.        |
| --help     |  -h   |    No    |            | Boolean |                                                                      | Displays command help. |

---

## Validation

Input validation is performed by the Application layer before the command handler is executed.

**Validator**:
- [UpdateSocialLinksCommandValidator](../../../../../src/WhoAmI.Application/Features/Profiles/UpdateSocialLinks/UpdateSocialLinksCommandValidator.cs)

---

## Examples

### Basic Example

Update the social links.

```shell
whoami-cli profile update social-links \
    -i b30545b1-b92b-4040-bbca-c733deb8b1c2 \
    -n https://www.linkedin.com/in/new-username \
    -g https://github.com/new-username
```

Expected output:

```text
Social links successfully updated.
```

> [!NOTE]
>
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
| [Profile Update Name](./name.md) | [Version](../../version.md) |

</div>

