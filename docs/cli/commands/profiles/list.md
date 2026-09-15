# Profile List

[🏠 Home](../../../../README.md) / [CLI](../../../../src/WhoAmI.CLI/README.md) / [Usage](../../usage.md) / **Profile List**

---

## Description

Displays a list of all stored profiles.

---

## Synopsis

```shell
whoami-cli profile list [options]
```

---

## Options

| Option  | Alias | Required |  Type   | Description            |
|:--------|:-----:|:--------:|:-------:|:-----------------------|
| --help  |  -h   |    No    | Boolean | Displays command help. |

---

## Validation

This command does not accept user input and therefore requires no validation.

---

## Examples

### Basic Example

List all profiles.

```shell
whoami-cli profile list
```

Expected output:

```text
╭───┬──────────────┬─────────────────────╮
│ # │ Name         │ Email               │
├───┼──────────────┼─────────────────────┤
│ 1 │ Sérgio Lopes │ email@example.com   │
│ 2 │ Sérgio Lopes │ contact@example.com │
╰───┴──────────────┴─────────────────────╯
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
| [Profile Get](./get.md) | [Profile Update Email](./update/email.md) |

</div>

