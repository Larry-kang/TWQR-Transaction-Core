# TWQR Transaction Core (Payment Gateway)

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/Larry-kang/TWQR-Transaction-Core)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-blue)](https://www.docker.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](./LICENSE)

> ⚠️ **DISCLAIMER: Educational Purpose Only**
>
> This project is a **Concept Proof (PoC)** demonstrating distributed system architecture (Idempotency, State Machine, Concurrency Control) using modern .NET technologies.
>
> * This project is **NOT** affiliated with, endorsed by, or connected to **FISC (Financial Information Service Co., Ltd.)** or **iPASS Corporation**.
> * The "TWQR" term is used solely to describe the compliance with public QR payment standards context.
> * All logic is implemented based on **general software engineering principles** and **publicly available documentation**, not proprietary source code.
> * No real money or financial transactions are processed.

---

<p align="center">
  <a href="#english-description">🇺🇸 English Description</a> | <a href="#chinese-description">🇹🇼 繁體中文介紹</a>
</p>

---

<a name="english-description"></a>
## 🇺🇸 English Description

> **High-Reliability Payment Simulation Engine adhering to TWQR (Taiwan QR Code) Standards.**
>
> This project serves as a reference implementation for a **Fault-Tolerant Payment Gateway**, demonstrating how to handle **Idempotency**, **Concurrency**, and **Distributed Transactions** in a high-throughput financial system.

### 🏗 System Architecture

The system is designed with **Eventual Consistency** and **Fail-Safe** mechanisms in mind. Below is the simplified transaction flow dealing with potential "Double-Spending" and upstream timeouts.

```mermaid
sequenceDiagram
    participant Client
    participant API as Payment Gateway (Idempotency Layer)
    participant Core as Transaction Core (FSM)
    participant DB as SQL Database
    participant Bank as Upstream Bank (Simulator)

    Note over Client, API: Phase 1: Request Validation
    Client->>API: POST /api/payment (Header: Idempotency-Key)
    API->>API: Check Redis Cache for Key
    alt Key Exists
        API-->>Client: 200 OK (Return Cached Result)
    else New Request
        API->>Core: Initiate Transaction
        Core->>DB: INSERT Transaction (State: CREATED)
        
        Note over Core, Bank: Phase 2: Execution & Locking
        Core->>DB: Pessimistic Lock / Version Check
        Core->>Core: Update State: LOCKED
        Core->>Bank: Call Deduct API
        
        alt Bank Success
            Core->>DB: Update State: COMPLETED
            API-->>Client: 200 OK (Success)
        else Bank Timeout / Network Error
            Core->>DB: Update State: UNKNOWN
            API-->>Client: 202 Accepted (Processing)
            Note right of Core: Background Worker will Reconcile later
        end
    end
