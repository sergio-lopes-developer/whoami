# Profile Create

> [CLI Documentation](../../../../src/WhoAmI.CLI/README.md) / [Usage](../../usage.md) / Profile Create

Creates a new profile in the local database.

| | |
|--|--|
| Command | `profile create` |
| Category | Profiles |
| Since | ![Version](https://img.shields.io/github/v/tag/<owner>/whoami) |
| Layer | CLI |

---

## Synopsis

```shell
whoami profile create [options]
```

Creates a new profile using the specified personal information.

---

## Options

| Options      | Alias | Required | Min Length | Max Length | Type  | Rules                                                                         | Description              |
|:-------------|:-----:|:--------:|:----------:|:----------:|:-----:|-------------------------------------------------------------------------------|:-------------------------|
| --first-name |  -f   |   Yes    |     2      |    100     | text  | [FirstName](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/FirstName.cs) | Profile's first name.    |
| --last-name  |  -l   |   Yes    |     2      |    100     | text  | [LastName](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/LastName.cs)   | Profile's last name.     |
| --email      |  -e   |   Yes    |            |            | email | [Email](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Email.cs)         | Profile's email address. |
| --linkedin   |  -n   |   Yes    |            |    255     |  URL  | [Url](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs)             | LinkedIn profile URL.    |
| --github     |  -g   |   Yes    |            |    255     |  URL  | [Url](../../../../src/WhoAmI.Domain/Profiles/ValueObjects/Url.cs)             | GitHub profile URL.      |

> **Note**
>
> All options are required.

---

## Validation

Input validation is performed by the application layer before the command is executed.

**Validator**:
- [CreateProfileCommandValidator](../../../../src/WhoAmI.Application/Features/Profiles/CreateProfile/CreateProfileCommandValidator.cs)

---

## Example

Create a new profile:

```
whoami profile create \
    -f John \
    -l Doe \
    -e john@email.com \
    -n https://linkedin.com/in/john \
    -g https://github.com/john
```

Expected output:

```
Profile successfully created.
```

---

<div align="center">

| &nbsp;&nbsp;&nbsp; ⬅️ Previous Page &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Next Page ➡️ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; |
| :----------------------------------------------------: | :------------------------------------------------------------------------: |
| [Usage](../../usage.md)                                | [Profile Get](./get.md)                                                      |

</div>

