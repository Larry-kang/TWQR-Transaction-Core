# 05. Reliability / Async / Idempotency

## 1. Reliability Goals

The platform assumes every external dependency may fail:

- Bank
- Card processor
- Blockchain
- Biller
- Redis
- RabbitMQ
- Network

Correctness rules:

- A retry must not duplicate money movement.
- An unknown result must remain recoverable.
- Message redelivery must not duplicate side effects.
- Redis failure must not break accounting correctness.

## 2. Idempotency Contract

All state-changing APIs require:

```http
Idempotency-Key: <client-generated-key>
```

Recommended identity scope:

```text
(Client / Merchant / Endpoint / IdempotencyKey)
```

Persist:

```text
IdempotencyKey
RequestHash
ResourceId
State
ResponseStatusCode
ResponseBody
CreatedAt
ExpiresAt
```

Behavior:

### New request

```text
Key not found
→ create processing record
→ execute business operation
→ persist final response
```

### Same key + same payload

```text
Completed
→ return original response

Processing
→ return current processing representation
```

### Same key + different payload

Return conflict. The same key must not be reused for another command.

The database unique constraint is the final correctness guard. Redis may accelerate lookup but is not the only protection.

## 3. Concurrency

V1 strategy:

- Unique constraints
- RowVersion / optimistic concurrency
- Conditional updates
- Database transactions
- Bounded retry for known safe concurrency conflicts

Distributed lock is optional coordination, not the primary correctness mechanism.

## 4. Sync-first Payment Response

Do not force every payment into asynchronous `202 Accepted`.

Preferred model:

```text
Request
  ↓
Process within latency budget
  ├─ final result available → 200/4xx
  └─ external result unresolved → 202 Processing / Unknown
```

Wallet-only payment should normally complete synchronously because the critical operation can remain within one platform-controlled transaction.

External card/bank operations may fall back to async completion.

## 5. Async Completion

Clients can observe asynchronous payment completion through:

- Polling: baseline
- Merchant webhook: V1
- SSE/WebSocket: optional UI enhancement

Webhook delivery requires:

- Delivery ID
- Signature
- Retry policy
- Idempotent receiver expectation
- Delivery log
- Dead-letter / terminal failure handling

## 6. Outbox Pattern

Critical flow:

```text
Business DB transaction
├─ Payment state
├─ Ledger / critical record
└─ Outbox message
        ↓
Background publisher
        ↓
RabbitMQ
```

The business transaction must not mark an event published before broker acknowledgement.

## 7. Inbox / Consumer Deduplication

Async consumers should record processed message identity before/with side effect.

Examples:

- Loyalty earn
- Notification
- Settlement aggregation
- External confirmation processing

At-least-once delivery is acceptable only when consumer side effects are idempotent.

## 8. Retry / Timeout

Every external call defines:

- Timeout
- Retry eligibility
- Max attempts
- Backoff
- Correlation ID
- Provider request ID

Do not blindly retry state-changing external operations unless provider-side idempotency or status inquiry exists.

## 9. Circuit Breaker

Circuit breaker is permitted for external simulators / providers to protect the system from repeated dependency failure.

Circuit breaker must not convert an unknown money state into failed.

## 10. Observability Minimum

Every payment path should expose:

```text
TraceId
CorrelationId
PaymentId
AttemptId
MerchantId
Provider
IdempotencyKey hash / safe representation
Status transition
Latency
ErrorCode
```

Never log card secret data or sensitive credentials.
