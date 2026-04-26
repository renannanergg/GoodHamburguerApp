# 🍔 Good Hamburguer App

> Solução para gestão de pedidos da lanchonete **Good Hamburguer**, desenvolvida com foco em alta performance, escalabilidade e Clean Architecture.

## 📋 Sobre o Projeto
O **Good Hamburguer App** é um ecossistema completo para o gerenciamento de pedidos. O sistema permite que os clientes da lanchonete montem combos personalizados, aproveitando uma lógica de descontos progressivos automatizada, além de gerenciar o histórico completo de suas compras, buscar pedidos específicos e realizar edições em tempo real.



## 🏗️ Arquitetura e Padrões de Projeto
A aplicação utiliza os princípios da **Clean Architecture** e do **Domain-Driven Design (DDD)**, garantindo que o núcleo da aplicação (Domínio) seja independente de tecnologias externas.

### Camadas do Sistema (8 Projetos):
* **Domain**: O coração da aplicação. Contém as entidades (como a classe `Pedido`), enums e exceções. Toda a lógica de cálculo de descontos e regras de negócio está protegida aqui.
* **Application**: Orquestra os casos de uso utilizando o padrão **CQRS** com a biblioteca **MediatR**.
* **Infra**: Implementação do acesso a dados, repositórios e integração com o banco de dados via Entity Framework Core.
* **IoC (Inversion of Control)**: Camada responsável pela Injeção de Dependência, isolando a configuração do restante do sistema.
* **Api**: Interface RESTful com versionamento (`v1.0`), documentação via Swagger, JWT e tratamento global de erros.
* **Web**: Frontend moderno e reativo desenvolvido em **Blazor WebAssembly**.
* **Tests (Unit & Integration)**: Suíte de testes automatizados para garantir a confiabilidade das regras de negócio e integrações.

### Padrões Utilizados:
* **CQRS**: Separação clara entre comandos (escrita) e consultas (leitura).
* **Cache-Aside (In-Memory)**: Implementação de cache para a consulta do cardápio, reduzindo o tempo de resposta e a carga no banco de dados.
* **Unit of Work**: Garante a atomicidade das operações no banco de dados.
* **Repository Pattern**: Abstração da persistência de dados para facilitar a testabilidade.
* **Global Exception Handling**: Tratamento centralizado de erros para respostas consistentes da API através de `ProblemDetails`.

## 💰 Regras de Negócio (Descontos Automatizados)
O sistema aplica descontos automaticamente com base na composição do combo selecionado pelo cliente no domínio:

| Composição do Combo | Desconto Aplicado |
| :--- | :--- |
| **Sanduíche + Batata + Refrigerante** | **20% de Desconto** |
| **Sanduíche + Refrigerante** | **15% de Desconto** |
| **Sanduíche + Batata** | **10% de Desconto** |

*Nota: O sistema valida a integridade do pedido, impedindo a duplicidade de categorias no mesmo combo.*

## 🛠️ Tecnologias e Frameworks
* **Backend**: .NET 9, ASP.NET Core API.
* **Frontend**: Blazor WebAssembly, Bootstrap 5.
* **ORM**: Entity Framework Core.
* **CQRS e Mapeamento**: MediatR e AutoMapper.
* **Banco de Dados**: SQL Server (Containerizado via Docker).
* **Segurança**: Autenticação via JWT (JSON Web Tokens).
* **Testes**: Xunit, Moq, FluentAssertions, Bogus.

---

---

## 📌 Endpoints da API (v1.0)

### 🔑 Autenticação
* `POST /api/auth/login`: Realiza a autenticação e retorna o Token JWT.

### 🍔 Cardápio (Acesso Livre)
* `GET /api/v1/itens/cardapio`: Lista itens do cardápio com suporte a paginação e **Cache-In-Memory**.

### 🛒 Pedidos (Requer Autenticação)
* `GET /api/v1/pedidos`: Lista todos os pedidos (paginado).
* `GET /api/v1/pedidos/{id}`: Detalhes de um pedido específico.
* `POST /api/v1/pedidos`: Criação de novo pedido.
* `PUT /api/v1/pedidos/{id}`: Atualização de itens de um pedido.
* `DELETE /api/v1/pedidos/{id}`: Exclusão de um pedido.

---

## 🔑 Credenciais de Acesso (Teste)
Para acessar as funcionalidades de pedidos no Frontend ou via Swagger, utilize as seguintes credenciais:

* **Usuário:** `admin`
* **Senha:** `123456`

---

## 🚀 Como Iniciar o Projeto

### Pré-requisitos
* Visual Studio 2022 ou VS Code.
* SDK do .NET 9.
* Docker Desktop (para o SQL Server).

### 1. Clonar o Repositório
```bash
git clone [https://github.com/renannanergg/GoodHamburguerApp.git](https://github.com/renannanergg/GoodHamburguerApp.git)

```

### 2. Subir o Ambiente com Docker

O projeto utiliza Docker para instanciar o SQL Server de forma rápida e isolada:


```Bash
docker-compose up -d
```

### 3. Aplicar as Migrations

Com as migrations presentes no projeto, execute o comando abaixo para criar o esquema do banco de dados:

```Bash
dotnet ef database update --project src/GoodHamburguerApp.Infra --startup-project src/GoodHamburguerApp.Api
```

### 4. Executar a Aplicação

Para rodar o projeto completo, você deve iniciar a API e o Cliente Web simultaneamente:

**Iniciar a API:**

``` Bash
cd src/GoodHamburguerApp.Api
dotnet run
```

**Iniciar o Web (Blazor):**

```Bash
cd src/GoodHamburguerApp.Web
dotnet run

```

----------

## 🧪 Testes

Para executar a suíte de testes unitários e de integração:

```Bash
dotnet test
```
----------



## 🚀 O que foi deixado de fora do Projeto


Embora o projeto esteja funcional e siga as melhores práticas de arquitetura, alguns melhorias foram deixadas para manter o foco no Domínio e na Arquitetura durante o desafio técnico:

---
### Mensageria (Event-Driven): 
Utilização de Kafka para processamento assíncrono de pedidos e comunicação entre microserviços.

--- 
### Idempotência de Requisições: 
Implementação de chaves de idempotência (Idempotency-Key) nos endpoints de criação de pedido para evitar duplicidade em caso de falhas de rede ou múltiplos cliques.

---
### Health Checks: 
Implementação de endpoints /health para monitoramento do status do banco de dados e serviços dependentes por ferramentas de orquestração.

### Refresh Tokens: 
Para uma gestão de sessão mais segura e fluida no frontend.

### Rate Limiting: 
Proteção contra ataques de força bruta ou excesso de requisições nos endpoints da API.






