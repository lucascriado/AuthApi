
# 🔐 Projeto AuthApi – API REST com JWT

Este projeto é uma API REST desenvolvida em **.NET 8** com autenticação via **JWT**. Ele oferece os seguintes recursos:

- ✅ Registro de usuários (`POST /auth/register`)
- ✅ Login de usuários (`POST /auth/login`)
- ✅ Rota protegida com autenticação JWT (`GET /auth/protected`)

As senhas são armazenadas com **SHA-256 hash**.

---

## Criação do `appsettings.json` no projeto

Modelo do arquivo `appsettings.json` com conexão ao banco de dados e chave JWT:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "server=127.0.0.1;port=xxxx;user=xxxx;password=xxxx;database=auth_db"
  },
  "JwtKey": "xxxx"
}
```

---

## Como testar no Swagger

1. Inicie o projeto:
   ```bash
   dotnet run
   ```

2. Acesse o Swagger:
   ```
   http://localhost:5143/swagger
   ```

3. Siga os passos abaixo:

---

### 1. Registrar usuário

**POST /auth/register**

```json
{
  "username": "user",
  "passwordHash": "xxxx"
}
```

---

### 2. Realizar login

**POST /auth/login**

```json
{
  "username": "user",
  "passwordHash": "xxxx"
}
```

**Resposta esperada:**

```json
{
  "token": "xxyyzz..."
}
```

---

### 3. Acessar rota protegida

**GET /auth/protected**

- Clique no botão `Authorize` no topo do Swagger
- Insira o token no formato:

```
Bearer xxyyzz...
```

- Envie a requisição

**Resposta esperada:**
```json
"Você está autenticado."
```
