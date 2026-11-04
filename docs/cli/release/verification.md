# Verify the Release

This release is signed using **Sigstore**.

The signatures are generated automatically by the GitHub Actions release workflow using Sigstore's keyless signing.

To verify the authenticity and provenance of the downloaded archive, you will need **Cosign**, the official Sigstore command-line tool.

---

## Install Cosign.

- **Windows**: Download `cosign-windows-amd64.exe` from: https://github.com/sigstore/cosign/releases

> [!NOTE]
>
> At the end of the **Assets** list, click **Show all** if the Windows binary is not immediately visible.

- **Linux**: Follow the official installation guide for your distribution: https://docs.sigstore.dev/cosign/system_config/installation/

- **macOS**: Install with Homebrew (recommended):

```shell
brew install cosign
```

---

## Download the release archive together with its corresponding `.sigstore.json` bundle.

The archive and its corresponding bundle must have matching names.

For example:

```shell
whoami-cli-1.0.0-linux-x64.tar.gz
whoami-cli-1.0.0-linux-x64.tar.gz.sigstore.json
```

---

## Verify the downloaded archive:

```shell
cosign verify-blob \
  --bundle <archive>.sigstore.json \
  <archive>
```

Replace `<archive>` with the name of the downloaded archive.

For example:

```shell
cosign verify-blob \
  --bundle whoami-cli-1.0.0-linux-x64.tar.gz.sigstore.json \
  whoami-cli-1.0.0-linux-x64.tar.gz
```

If the verification succeeds, Cosign confirms that the archive's signature is valid and that it was signed by the GitHub Actions workflow for this repository.

Unlike traditional code signing, Sigstore's keyless signing does not require users to obtain or manage public keys to verify the downloaded archive.

---
