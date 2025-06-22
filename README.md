# RabbitMQPubSub101

A simple playground project demonstrating the basics of Publish/Subscribe (Pub/Sub) messaging with [RabbitMQ](https://www.rabbitmq.com/) in .NET. This solution contains a producer and two independent consumers, each implemented as a separate console application. The project is intended for learning and demonstration purposes and will be published to GitHub.

---

## Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [How It Works](#how-it-works)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Running RabbitMQ](#running-rabbitmq)
  - [Building the Solution](#building-the-solution)
  - [Running the Producer and Consumers](#running-the-producer-and-consumers)
- [Code Highlights](#code-highlights)
- [License](#license)

---

## Overview

This project demonstrates the classic Pub/Sub pattern using RabbitMQ as the message broker. The producer publishes messages to a fanout exchange, and each consumer receives a copy of every message by listening to its own queue bound to the exchange.

---

## Project Structure

```
RabbitMQPubSub101/
│
├── src/
│   ├── Producer/
│   │   ├── Producer.csproj
│   │   └── Program.cs
│   └── Consumers/
│       ├── ConsumerOne/
│       │   ├── ConsumerOne.csproj
│       │   └── Program.cs
│       └── ConsumerTwo/
│           ├── ConsumerTwo.csproj
│           └── Program.cs
├── RabbitMQPubSub101.sln
├── README.md
└── LICENSE
```

- **Producer**: Publishes messages to the `messages` exchange. Located at `src/Producer`.
- **ConsumerOne**: Listens to the `messageOne` queue. Located at `src/Consumers/ConsumerOne`.
- **ConsumerTwo**: Listens to the `messageTwo` queue. Located at `src/Consumers/ConsumerTwo`.

---

## How It Works

- The **Producer** connects to RabbitMQ and publishes messages to a durable fanout exchange named `messages`.
- Each **Consumer** declares its own durable queue (`messageOne` or `messageTwo`) and binds it to the `messages` exchange.
- Because the exchange is of type `fanout`, every message published by the producer is delivered to all bound queues, so both consumers receive every message.

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [RabbitMQ](https://www.rabbitmq.com/download.html) running locally (default settings)
- [Docker](https://www.docker.com/) (optional, for running RabbitMQ easily)

### Running RabbitMQ

You can run RabbitMQ locally using Docker:

```sh
docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

- Management UI: http://localhost:15672 (default user/pass: guest/guest)
- AMQP port: 5672

### Building the Solution

From the root directory:

```sh
dotnet build RabbitMQPubSub101.sln
```

### Running the Producer and Consumers

Open three terminals and run each project:

**Terminal 1: Producer**

```sh
dotnet run --project src/Producer/Producer.csproj
```

**Terminal 2: ConsumerOne**

```sh
dotnet run --project src/Consumers/ConsumerOne/ConsumerOne.csproj
```

**Terminal 3: ConsumerTwo**

```sh
dotnet run --project src/Consumers/ConsumerTwo/ConsumerTwo.csproj
```

You should see both consumers printing every message published by the producer.

---

## Code Highlights

### Producer

- Connects to RabbitMQ and declares a fanout exchange.
- Publishes 5 messages, one per second, with persistent delivery mode.

See `src/Producer/Program.cs`.

### Consumers

- Each consumer declares its own queue and binds it to the `messages` exchange.
- Uses `AsyncEventingBasicConsumer` for asynchronous message handling.
- Acknowledges each message after processing.

See `src/Consumers/ConsumerOne/Program.cs` and `src/Consumers/ConsumerTwo/Program.cs`.

---

## Exchange Types in RabbitMQ

While this project uses a **fanout** exchange (which broadcasts all messages to all bound queues), RabbitMQ supports several other exchange types, each with different routing logic:

- **Direct Exchange**: Routes messages to queues based on an exact match between the routing key and the queue binding key.
- **Topic Exchange**: Routes messages to queues based on pattern matching between the routing key and the binding pattern (supports wildcards).
- **Headers Exchange**: Routes messages based on message header values instead of the routing key.
- **Fanout Exchange**: (Used in this project) Broadcasts all messages to all queues bound to the exchange, ignoring routing keys.
- **Default Exchange**: A special direct exchange with no name, allowing messages to be delivered directly to a queue with a name matching the routing key.

For more details, see the [RabbitMQ Exchange Types documentation](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange).

---

## License
[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

This project is licensed under the MIT License. See the [`LICENSE`](LICENSE) file for details.