# ByteShop - Arquitetura de Microserviços Baseada em Eventos

Este projeto consiste em um ecossistema de e-commerce robusto e distribuído, desenvolvido em **.NET 8** utilizando os princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **Event-Driven Architecture (EDA)**. 

O objetivo principal deste projeto é demonstrar a aplicação prática de padrões de resiliência, desacoplamento e a estratégia de **Polyglot Storage** (armazenamento poliglota), onde cada microserviço utiliza a tecnologia de banco de dados mais adequada para o seu modelo de negócio.

---

## 🏗️ Arquitetura Geral do Sistema

O ecossistema é composto por microserviços isolados que se comunicam de forma assíncrona através de um Message Broker. A blindagem do domínio e o encapsulamento estrito são mantidos em todas as camadas através do SOLID.

### 🧩 Microserviços Construídos

1. **ByteShop.Catalog.API (Serviço de Catálogo)**
   * **Responsabilidade:** Gerenciamento de produtos, categorias e buscas.
   * **Persistência:** **MongoDB** (NoSQL orientado a documentos), ideal para cenários de alta leitura, buscas rápidas e esquemas flexíveis de produtos.
   
2. **ByteShop.Cart.API (Serviço de Carrinho)**
   * **Responsabilidade:** Armazenamento temporário e manipulação dos itens de intenção de compra dos clientes.
   * **Persistência:** **Redis** (Armazenamento em cache estruturado em memória), garantindo latência ultrabaixa para operações frequentes de adição e remoção de itens.
   * **Blindagem:** Uso de propriedades encapsuladas (`private set`) com mapeamento explícito de desserialização via `[JsonInclude]`.

3. **ByteShop.Ordering.API (Serviço de Pedidos)**
   * **Responsabilidade:** Processamento transacional de checkout, controle de status de pedidos e faturamento.
   * **Persistência:** **SQL Server** utilizando **Entity Framework Core 8**.
   * **Padrões:** Implementação de **Aggregate Roots** (Raízes de Agregação). Itens de pedido não possuem repositório próprio e são manipulados estritamente através da entidade principal `Pedido`. Mapeamento isolado na Infraestrutura através de **Fluent API** e uso de *Backing Fields* para coleções de leitura exclusivas (`IReadOnlyCollection`).

4. **ByteShop.BuildingBlocks.EventBus (Mensageria e Integração)**
   * **Responsabilidade:** Projeto centralizado contendo os contratos de eventos de integração compartilhados (`records` imutáveis).
   * **Tecnologia:** **RabbitMQ** como Message Broker, orquestrado através do **MassTransit** para gerenciamento automatizado de filas, exchanges e topologias de consumo.

---

## 🔄 Fluxo Assíncrono Orientado a Eventos (EDA)

Para garantir o desacoplamento total entre o momento da compra e a manutenção do carrinho de compras, foi implementado o seguinte fluxo:

1. O cliente realiza o fechamento da compra consumindo a `Ordering.API`.
2. O pedido é persistido de forma transacional (ACID) no **SQL Server**.
3. Imediatamente após o sucesso da transação, a camada de aplicação publica o evento `PedidoCriadoIntegrationEvent` no **RabbitMQ** através do MassTransit.
4. A `Cart.API`, que possui um `PedidoCriadoConsumer` ativo escutando o broker, intercepta a mensagem de forma assíncrona e executa a limpeza automática do carrinho do cliente no **Redis**.

---

## 🛠️ Tecnologias e Ferramentas Utilizadas

* **Framework Principal:** .NET 8 (C# 12)
* **Acesso a Dados & ORM:** Entity Framework Core 8, MongoDB Official Driver, StackExchange.Redis
* **Mensageria:** RabbitMQ & MassTransit
* **Documentação:** Swagger / OpenAPI UI
* **Ambiente Local:** Docker & Docker Compose
* **IDE Recomendada:** Visual Studio 2022 / VS Code

---

## 🗂️ Estrutura de Solução Padrão (Clean Architecture / DDD)

Cada microserviço respeita rigidamente a divisão de camadas abaixo:

```text
📁 src
   📁 Services
      📁 NomeDoServico
         ├── 🔵 NomeDoServico.API          (Controllers, Configuração de DI, Program.cs)
         ├── 🟢 NomeDoServico.Application  (Casos de Uso, Serviços de Aplicação, DTOs, Consumers)
         ├── 🟠 NomeDoServico.Infrastructure (Contexto de Banco, Repositórios, Mapeamentos)
         └── 🔴 NomeDoServico.Domain         (Entidades Ricas, Enums, Regras de Negócio, Interfaces)
