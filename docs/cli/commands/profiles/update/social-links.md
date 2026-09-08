# Profile Update Social Links

[🏠 Home](../../../../../README.md) / [CLI](../../../../../src/WhoAmI.CLI/README.md) / [Usage](../../../usage.md) / **Profile Update Social Links**

---

## Description

Updates the social links of a profile.

---

## Synopsis

```shell
whoami-cli profile update social-links [options]
```

---

## Options

| Options    | Alias | Required | Max Length |  Type   | Domain Type                                                       | Description            |
|:-----------|:-----:|:--------:|:----------:|:-------:|:------------------------------------------------------------------|:-----------------------|
| --id       |  -i   |   Yes    |            |  guid   |                                                                   | Profile identifier.    |
| --linkedin |  -n   |   Yes    |    255     |   URL   | [Url](../../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs) | Profile LinkedIn URL.  |
| --github   |  -g   |   Yes    |    255     |   URL   | [Url](../../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs) | Profile GitHub URL.    |
| --help     |  -h   |    No    |            | boolean |                                                                   | Displays command help. |

---

## Validation

Input validation is performed by the application layer before the command is executed.

**Validator**:
- [UpdateSocialLinksCommandValidator](../../../../../src/WhoAmI.Application/Features/Profiles/UpdateSocialLinks/UpdateSocialLinksCommandValidator.cs)

---

## Examples

### Minimal

Update the social links of a profile.

```shell
whoami-cli profile update social-links \
    -i b30545b1-b92b-4040-bbca-c733deb8b1c2 \
    -n https://linkedin.com/in/new-username \
    -g https://github.com/new-username
```

Expected output:

```text
Social links successfully updated.
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
| [Profile Update Name](./name.md) | [Version](../../version.md) |

</div>

