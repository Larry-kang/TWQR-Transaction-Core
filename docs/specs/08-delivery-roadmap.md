# 08. Delivery Roadmap

## Delivery Strategy

Use:

```text
POC
→ verify invariants
→ small integrated flow
→ failure simulation
→ automation
→ polish
```

Do not implement every domain simultaneously.

## Phase 0 — Specification & Baseline

- Confirm product scope
- Confirm bounded contexts
- Confirm payment state machine
- Confirm ledger invariants
- Define migration path from current skeleton
- Align README with implemented reality

Exit criteria:

- Spec reviewed
- No major unresolved payment / ledger ambiguity

## Phase 1 — Payment + Ledger Core

Implement:

- PaymentOrder
- PaymentAttempt
- Payment FSM
- Wallet
- Double-entry ledger
- RowVersion
- Unit tests

Demo:

```text
Wallet = 100
Payment = 80
→ one successful payment
→ ledger balanced
→ concurrent duplicate/spend cannot overdraw
```

## Phase 2 — Idempotency + QR

Implement:

- durable idempotency record
- request hash
- original response replay
- DB unique constraint
- merchant-presented QR
- consumer-presented token
- replay protection

Demo:

- duplicate HTTP retry does not duplicate payment
- same key + different payload rejected
- QR token cannot be reused

## Phase 3 — Card Simulator + Unknown

Implement:

- ICardProcessor
- simulator scenarios
- timeout handling
- Payment Unknown
- reconciliation worker

Demo:

```text
Card processed
response timeout
→ 202 / Unknown
→ reconcile
→ Succeeded
```

## Phase 4 — Outbox + RabbitMQ

Implement:

- Outbox
- publisher worker
- RabbitMQ
- Inbox / dedupe
- PaymentSucceeded event

Consumers:

- loyalty
- notification
- merchant settlement preparation

## Phase 5 — Merchant Settlement

Implement:

- receivable
- fee
- settlement batch
- settlement simulator
- settlement reconciliation

Demo:

- multiple payments
- refund
- cutoff
- net settlement
- failed payout retry / reconcile

## Phase 6 — Funding + Bill Payment

Implement:

- bank funding simulator
- wallet credit
- biller simulator
- bill query
- bill payment confirmation

## Phase 7 — Frontends

Implement minimum usable:

- Consumer PWA
- Merchant POS PWA
- Merchant Admin

Focus on backend demoability rather than UI polish.

## Phase 8 — Production-Oriented Hardening

- Redis acceleration / coordination
- OpenTelemetry
- structured logs
- metrics
- health checks
- timeout / retry policies
- load tests
- integration tests
- architecture tests
- Docker Compose
- SQL Server
- simulator fault injection

## V1.5

- Crypto deposit simulator
- confirmations
- mock FX quote
- wallet credit
- Platform Ops Console expansion

## Explicit Non-goals for Early Phases

- Microservices split
- Kafka
- Kubernetes
- real card data
- real banking integration
- real blockchain custody

These may be documented later but must not delay core payment correctness.
