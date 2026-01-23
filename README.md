# GregCustomers

Prova de Conceito (POC) para cadastro de clientes, desenvolvida como parte do desafio técnico da Thomas Greg.  
A aplicação permite o gerenciamento de clientes e seus logradouros, incluindo upload de logotipo, autenticação e autorização, e atenção especial a performance e integridade de dados.

A solução é composta por:

- Back-end: API REST em ASP.NET Core (.NET 8)
- Front-end: ASP.NET Core MVC (Razor)
- Banco de dados: SQL Server
- Persistência híbrida:
    - Escrita (Commands): Stored Procedures + Dapper
    - Leitura (Queries): Entity Framework Core

---

## Requisitos atendidos

- Cadastro, edição, consulta e remoção de clientes
- Cadastro, edição, consulta e remoção de logradouros
- Um cliente pode possuir múltiplos logradouros
- Não permitir clientes com e-mail duplicado
- Não permitir o mesmo logradouro duplicado para o mesmo cliente
- Upload e download de logotipo do cliente armazenado no banco de dados
- API acessível publicamente, protegida por autenticação e autorização
- Utilização de JWT (JSON Web Token)
- Preocupação com performance e boas práticas de acesso a dados

---

## Stack utilizada

- .NET 8 SDK
- ASP.NET Core Web API
- ASP.NET Core MVC
- MediatR
- Dapper
- Entity Framework Core
- SQL Server 2016+
- Docker / Docker Compose

---

## Estrutura do repositório

- `src/GregCustomers.Api`  
  API REST responsável pelas regras de negócio e acesso a dados

- `src/GregCustomers.WebMvc`  
  Aplicação MVC responsável pela interface web

- `src/GregCustomers.Application`  
  Casos de uso, Commands, Queries e contratos

- `src/GregCustomers.Domain`  
  Entidades e regras de domínio

- `src/GregCustomers.Infrastructure`  
  Implementações de persistência (EF Core e Dapper)

- `database/scripts`  
  Scripts SQL para criação de tabelas, índices e stored procedures

- `docs`  
  Documentação técnica do projeto

---

## Como executar o projeto localmente

### Pré-requisitos

- Docker
- .NET 8 SDK
- SQL Server Management Studio ou Azure Data Studio (opcional)

---

### 1) Subir o SQL Server com Docker

Na raiz do projeto, execute:

```bash
docker compose up -d
```

### Configuração do banco de dados

Configuração utilizada no ambiente local:

- Host: `localhost`
- Porta: `1433`
- Usuário: `sa`
- Senha: `GregCustomer@123`
- Database: `GregCustomersDb`

Obs.: a senha está no `compose.yaml` por conveniência de execução local. Em cenários reais, use Secrets/variáveis de ambiente.

---

### 2) Criar tabelas, índices e procedures

Execute os scripts SQL abaixo **na ordem**:

1. `database/scripts/001_create_tables.sql`
2. `database/scripts/002_stored_procs.sql`

---

### 3) Configurar autenticação JWT

Arquivo:

```
src/GregCustomers.Api/appsettings.Development.json
```

Configuração:

```json
"Jwt": {
  "Issuer": "GregCustomers",
  "Audience": "GregCustomers",
  "Key": "SUA_CHAVE_SECRETA_COM_NO_MINIMO_32_CARACTERES",
  "ExpiresMinutes": 60
}
```

Antes de rodar, substitua `Jwt:Key` por uma chave longa e aleatória (32+ caracteres).

---

### 4) Executar a API

```bash
cd src/GregCustomers.Api
dotnet run
```

A API ficará disponível em:

- Swagger: `http://localhost:5181/swagger`
- Base URL: `http://localhost:5181`

---

### 5) Gerar token JWT

Endpoint:

```
POST /api/auth/login
```

Usuários de teste:

| Usuário | Senha     | Role   |
|--------|-----------|--------|
| admin  | admin123  | admin  |
| reader | reader123 | reader |

Exemplo de chamada:

```bash
curl -X POST http://localhost:5181/api/auth/login   -H "Content-Type: application/json"   -d "{\"username\":\"admin\",\"password\":\"admin123\"}"
```

Copie o valor do `access_token` retornado.

Como usar o token:

- Swagger: botão **Authorize** -> `Bearer {token}`
- Front-end MVC: configure em `src/GregCustomers.WebMvc/appsettings.json` (próximo passo)

---

### 6) Executar o Front-end MVC

Arquivo:

```
src/GregCustomers.WebMvc/appsettings.json
```

Configuração:

```json
"Api": {
  "BaseUrl": "http://localhost:5181",
  "JwtToken": "COLE_AQUI_UM_TOKEN_VALIDO"
}
```

Execução:

```bash
cd src/GregCustomers.WebMvc
dotnet run
```

A aplicação estará disponível em:

```
http://localhost:5161
```

---

## Endpoints principais da API

### Autenticação

- `POST /api/auth/login`

### Clientes

- `GET /api/clients`
- `GET /api/clients/{id}`
- `POST /api/clients` (admin)
- `PUT /api/clients/{id}` (admin)
- `DELETE /api/clients/{id}` (admin)

### Logo do cliente

- `POST /api/clients/{id}/logo`
- `GET /api/clients/{id}/logo`

### Logradouros

- `GET /api/clients/{clientId}/addresses`
- `POST /api/clients/{clientId}/addresses` (admin)
- `PUT /api/addresses/{id}` (admin)
- `DELETE /api/addresses/{id}` (admin)

---

## Integridade e regras no banco

- E-mail único por cliente (índice único)
- Logradouro único por cliente (índice único composto)

---

## Tratamento de erros

A API retorna erros HTTP padronizados para os cenários abaixo:

- 400: validações e erros de entrada
- 401/403: autenticação/autorização
- 404: recurso não encontrado
- 409: violação de unicidade (e-mail/logradouro duplicado)
- 500: erro inesperado

---

## Considerações de performance

- Índices únicos no banco de dados
- Paginação na listagem de clientes
- Consultas com `AsNoTracking`
- Escritas realizadas via stored procedures

---

## Documentação adicional

- `docs/api.md`: detalhes de API e comportamento
- `docs/decisions.md`: decisões técnicas (ORM + stored procedures, logo no banco, JWT)
- `docs/architecture.md`: visão de arquitetura (camadas e fluxo)

---

## Troubleshooting

- Erro de conexão com SQL Server:
    - Verifique se o container está de pé: `docker ps`
    - Confirme a porta 1433 livre
- Endpoints de escrita retornando 401/403:
    - Verifique se você está usando token válido com role `admin`
- Conflito (409) ao criar cliente/endereço:
    - E-mail duplicado ou logradouro duplicado para o mesmo cliente

---

## Licença

Projeto desenvolvido exclusivamente para avaliação técnica.