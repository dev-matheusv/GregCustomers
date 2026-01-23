# Arquitetura da Solução

Este documento descreve a arquitetura adotada no projeto GregCustomers, bem como as principais decisões técnicas.

---

## Visão Geral

A aplicação foi desenvolvida seguindo os princípios de separação de responsabilidades, clareza de fluxo e facilidade de manutenção.  
A solução é dividida em camadas bem definidas, com comunicação clara entre elas.

---

## Camadas da aplicação

### Domain

- Contém as entidades principais do sistema
- Regras de negócio centrais
- Não depende de nenhuma outra camada

Exemplos:
- Client
- Address

---

### Application

- Contém os casos de uso da aplicação
- Implementação de Commands e Queries
- Utiliza MediatR para desacoplamento
- Define contratos (interfaces) para persistência

Responsabilidades:
- Orquestrar regras de negócio
- Validar fluxo de execução
- Não acessa diretamente banco de dados

---

### Infrastructure

- Implementação concreta do acesso a dados
- Leitura via Entity Framework Core
- Escrita via Dapper + Stored Procedures
- Implementações de repositórios e contextos

Essa abordagem híbrida foi adotada visando:
- Melhor performance nas operações de escrita
- Flexibilidade e simplicidade nas consultas

---

### API

- Camada de entrada do sistema
- Controllers responsáveis por expor os endpoints REST
- Autenticação e autorização via JWT
- Middleware global para tratamento de erros

---

### WebMvc

- Interface web desenvolvida com ASP.NET Core MVC
- Consome a API via HttpClientFactory
- Autenticação baseada em token JWT
- Responsável apenas por apresentação e interação com o usuário

---

## Autenticação e Autorização

- JWT (JSON Web Token)
- Roles:
    - admin: permissões de escrita
    - reader: apenas leitura
- Tokens gerados via endpoint de autenticação

---

## Persistência de dados

- SQL Server
- Índices únicos para garantir integridade
- Stored procedures para escrita
- EF Core com consultas otimizadas para leitura

---

## Tratamento de erros

- Middleware global
- Retorno padronizado de erros HTTP
- Códigos utilizados:
    - 400: erro de validação
    - 401/403: autorização
    - 404: recurso não encontrado
    - 409: violação de regra de unicidade
    - 500: erro inesperado

---

## Considerações finais

A arquitetura foi escolhida visando clareza, manutenibilidade e aderência aos requisitos do desafio, sem introduzir complexidade desnecessária.