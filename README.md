# ByteShop - Arquitetura de Microserviços Baseada em Eventos

Este projeto consiste num ecossistema de e-commerce robusto e distribuído, desenvolvido em **.NET 8** utilizando os princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **Event-Driven Architecture (EDA)**. 

O objetivo principal deste projeto é demonstrar a aplicação prática de padrões de resiliência, desacoplamento, orquestração de contentores e a estratégia de **Polyglot Storage** (armazenamento poliglota).

---

## 🏗️ Arquitetura Geral do Sistema

O ecossistema é composto por microserviços isolados que comunicam de forma assíncrona através de um Message Broker, centralizados por um API Gateway. A blindagem do domínio e o encapsulamento estrito são mantidos em todas as camadas através do SOLID.

### 🧩 Peças de Infraestrutura e Serviços

1. **ByteShop.ApiGateway (Ponto de Entrada Único)**
   * **Responsabilidade:** Atuar como proxy reverso, centralizando as chamadas do front-end e encaminhando-as para os microserviços corretos de forma transparente.
   * **Tecnologia:** **YARP (Yet Another Reverse Proxy)**.

2. **ByteShop.Catalog.API (Serviço de Catálogo)**
   * **Persistência:** **MongoDB** (NoSQL orientado a documentos), ideal para cenários de alta leitura e esquemas flexíveis de produtos.
   
3. **ByteShop.Cart.API (Serviço de Carrinho)**
   * **Persistência:** **Redis** (Armazenamento em cache estruturado em memória), garantindo latência ultrabaixa para manipulação rápida de intenções de compra.
   * **Resiliência:** Tolerância a falhas na ligação inicial ao Redis (`AbortOnConnectFail=false`).

4. **ByteShop.Ordering.API (Serviço de Pedidos)**
   * **Persistência:** **SQL Server** utilizando **Entity Framework Core 8**. Transacional (ACID) e relacional.
   * **Padrões:** Implementação rigorosa de **Aggregate Roots** (Raízes de Agregação).

5. **ByteShop.BuildingBlocks.EventBus (Mensageria e Integração)**
   * **Tecnologia:** **RabbitMQ** orquestrado através do **MassTransit**.
   * **Resiliência Implementada:** Políticas de Retry (Tentativas), Circuit Breaker (Disjuntor para proteção de rede) e Dead Letter Queues (DLQ) para mensagens não processadas.

---

## 🔄 Fluxo Assíncrono Orientado a Eventos (EDA)

Para garantir o desacoplamento total entre o momento da compra e a manutenção do carrinho de compras, implementou-se o seguinte fluxo:

1. O cliente realiza o fecho da compra consumindo o Gateway (porta `7108`), que encaminha para a `Ordering.API`.
2. O pedido é persistido transacionalmente no **SQL Server**.
3. Imediatamente, a aplicação publica o evento `PedidoCriadoIntegrationEvent` no **RabbitMQ**.
4. A `Cart.API` interceta a mensagem de forma assíncrona e executa a limpeza automática do carrinho no **Redis**.

---

## 🛠️ Tecnologias e Ferramentas Utilizadas

* **Framework Principal:** .NET 8 (C# 12)
* **Acesso a Dados & ORM:** Entity Framework Core 8, MongoDB Official Driver, StackExchange.Redis
* **Mensageria:** RabbitMQ & MassTransit
* **Proxy Reverso:** YARP
* **Ambiente Local e Orquestração:** Docker & Docker Compose (Multi-stage builds)
* **Documentação da API:** Swagger / OpenAPI UI

---

## 🚀 Como Executar a Infraestrutura Localmente

Todo o ecossistema (Bancos de Dados, Mensageria e APIs .NET) está encapsulado em contentores Docker e orquestrado através do `docker-compose`.

### Pré-requisitos
* Docker Desktop instalado e em execução.

### Passos para a Execução
1. Navegue até à pasta do ambiente local onde se encontra o ficheiro compose:
   ```bash
   cd local-environment

   docker compose up -d --build

🌐 Endpoints Principais

API Gateway: http://localhost:7108 (Use as rotas /gateway/carrinho/ ou /gateway/pedidos/)

RabbitMQ Management UI: http://localhost:15672 (guest / guest)

Swagger - Pedidos: http://localhost:7110/swagger

Swagger - Carrinho: http://localhost:7011/swagger
