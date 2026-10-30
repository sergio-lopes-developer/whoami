# WhoAmI CLI Release Guide

Thank you for downloading **WhoAmI CLI**.

The application is distributed as a self-contained executable that includes the required .NET runtime. No additional installation is necessary.

Make sure you download the archive that matches your operating system and processor architecture.

---

## Running

### Windows

Run the following command:

```text
whoami-cli.exe profile list
```

### Linux/macOS

The executable is distributed with executable permissions and should be ready to run after extracting the archive.

Run the following command:

```text
./whoami-cli profile list
```

If your archive extraction tool does not preserve executable permissions, run:

```text
chmod +x whoami-cli
```

Then run the application again.

---

## Documentation

To learn the available commands, see the [Usage documentation](https://github.com/sergio-lopes-developer/whoami/blob/<GIT_TAG>/docs/cli/usage.md).

The application uses `appsettings.json` to configure the database connection and other settings.

If you need to customize its behavior before running the application, see the [Configuration documentation](https://github.com/sergio-lopes-developer/whoami/blob/<GIT_TAG>/docs/cli/configuration.md).

---

## License

This project is licensed under the MIT License. See the included LICENSE file for details.
