# Profile Delete

[🏠 Home](../../../../README.md) / [CLI](../../../../src/WhoAmI.CLI/README.md) / [Usage](../../usage.md) / **Profile Delete**

---

## Description

Deletes a profile.

---

## Synopsis

```shell
whoami-cli profile delete [options]
```

---

## Options

| Option | Alias | Required |  Type   | Description            |
|:-------|:-----:|:--------:|:-------:|:-----------------------|
| --id   |  -i   |   Yes    |  GUID   | Profile identifier.    |
| --help |  -h   |    No    | Boolean | Displays command help. |

> [!TIP]
>
> This command requires the profile ID.
>
> To retrieve it, run:
>
> ```shell
> whoami-cli profile get --email <EMAIL_ADDRESS> --verbose
> ```
>
> The `--verbose` option displays the profile identifier required by this command.

---

## Validation

Input validation is performed by the Application layer before the command handler is executed.

**Validator**:
- [DeleteProfileCommandValidator](../../../../src/WhoAmI.Application/Features/Profiles/DeleteProfile/DeleteProfileCommandValidator.cs)

---

## Examples

### Basic Example

Delete a profile.

```shell
whoami-cli profile delete \
    -i b30545b1-b92b-4040-bbca-c733deb8b1c2
```

Expected output:

```text
Profile successfully deleted.
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
| [Profile Create](./create.md) | [Profile Get](./get.md) |

</div>
