# 🗓️ Sistema de Agendamentos para Serviços

Este projeto é uma API para gerenciamento genérico de agendamentos de serviços, desenvolvido em .NET 9 com foco em arquitetura limpa, boas práticas de autenticação/autorização, e extensibilidade.

## ✅ Funcionalidades Principais

- 📅 Cadastro e gerenciamento de agendamentos
- 👥 Registro e autenticação de usuários com ASP.NET Identity
- 🔒 Autenticação via JWT
- 🔄 Mapeamento de DTOs com AutoMapper
- 💬 Comunicação desacoplada com MediatR **(CQRS)**
- 🧱 Estrutura modular com Minimal APIs
- 💽 Integração com banco de dados SQL Server via Entity Framework Core
- 🛡️ Validação de permissões e rotas protegidas por role
- 📦 Arquitetura escalável e extensível

---

## 🛠️ Tecnologias e Conceitos Utilizados

| Tecnologia       | Descrição                                                                 |
|------------------|---------------------------------------------------------------------------|
| **.NET 9**        | Base do projeto, com uso de recursos modernos como Minimal APIs           |
| **Entity Framework Core** | ORM para mapeamento do banco SQL e migrations                     |
| **ASP.NET Identity** | Gerenciamento de usuários, roles e autenticação                        |
| **JWT (Json Web Token)** | Autenticação baseada em tokens para segurança das rotas            |
| **MediatR**       | Implementação de CQRS e comunicação desacoplada (comandos/queries)       |
| **AutoMapper**    | Mapeamento automático entre entidades e DTOs                             |
| **Minimal APIs**  | Definição enxuta e performática de endpoints HTTP                        |

---

## 📐 Arquitetura

O projeto está dividido por responsabilidades, seguindo princípios da Clean Architecture:
```
/Api -> Camada de apresentação com endpoints definidos via Minimal APIs
│
├── Application -> Handlers (via MediatR), Commands, Querys, ViewModels, Mappings e DTOs
│
├── Domain -> Entidades e contratos (interfaces) do domínio
│
├── Infrastructure -> Acesso a dados (Entity Framework), configuração do Identity, etc.
│
└── Services -> Regras de negócio como AuthService (JWT), validações de e-mail/CNPJ, etc.
```

---

## 🔐 Autenticação e Segurança

- **Cadastro/Login** com gerenciamento de credenciais via Identity
- **Geração de Tokens JWT** com roles e claims
- **Proteção de Endpoints** com `[Authorize]` e validação de permissões

---

## 🧪 Testes (em construção)

Foco nos testes de unidade e integração com cobertura para Handlers, Services e validações.
