### Working with docker.

There are some prerequisites for using the included Dockerfile and docker-compose.yml files:

1) Make sure you have docker installed (on windows install docker desktop)

2) Create and install an https certificate:

    ```
    dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx" -p SuperSecurePassword123!
    ```

3) It's possible that the above step gives you an `A valid HTTPS certificate is already present.` error.
   In that case you will have to run this first:

    ```
     dotnet dev-certs https --clean
    ```

4) Trust the certificate

    ```
     dotnet dev-certs https --trust
    ```


After that you should be able to run

     docker-compose up -d --build

from the root project folder and if everything builds fine, your api should be available at `https://localhost:5060/swagger`

---

### WSL2 (Linux)

`dotnet dev-certs https -ep` does not work reliably on WSL2. Use `openssl` instead:

1) Fix ownership of the `.aspnet` directory if it was created as root:

    ```bash
    sudo chown -R $USER ~/.aspnet
    ```

2) Generate a self-signed certificate and export it as a PFX:

    ```bash
    mkdir -p ~/.aspnet/https && \
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
      -keyout /tmp/aspnetapp.key \
      -out /tmp/aspnetapp.crt \
      -subj "/CN=localhost" \
      -addext "subjectAltName=DNS:localhost" && \
    openssl pkcs12 -export \
      -out ~/.aspnet/https/aspnetapp.pfx \
      -inkey /tmp/aspnetapp.key \
      -in /tmp/aspnetapp.crt \
      -passout pass:SuperSecurePassword123! && \
    rm /tmp/aspnetapp.key /tmp/aspnetapp.crt
    ```

3) Make the certificate readable by the Docker container user:

    ```bash
    chmod 644 ~/.aspnet/https/aspnetapp.pfx
    ```

---

For advanced deployment scenarios, consider adding environment-specific `docker-compose.override.yml` files alongside the root `docker-compose.yml`.