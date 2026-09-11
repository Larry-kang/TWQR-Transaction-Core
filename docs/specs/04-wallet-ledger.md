# 04. Wallet & Ledger

## 1. Accounting Principle

The platform must not rely on `Wallet.Balance` as the sole source of truth.

The accounting source of truth is the **double-entry ledger**.

Core entities:

```text
LedgerAccount
LedgerTransaction
LedgerEntry
```

Invariant:

```text
SUM(Debit) == SUM(Credit)
```

for every committed ledger transaction.

## 2. Ledger Rules

- Ledger entries are append-only.
- Posted accounting entries must not be edited or deleted.
- Corrections use compensating transactions.
- Monetary type uses decimal.
- Each posting has business reference id.
- Each business operation must be idempotently postable.
- Ledger transaction commit and payment-critical state update should share an atomic transaction where feasible.

## 3. Suggested Accounts

V1 examples:

```text
Customer Wallet Liability
Merchant Payable
Platform Fee Revenue
Settlement Clearing
Bank Funding Clearing
Card Clearing
Refund Clearing
```

## 4. Wallet Payment Example

Consumer pays NT$100:

```text
Dr Customer Wallet Liability   100
Cr Merchant Payable            100
```

If platform fee is NT$3:

```text
Dr Merchant Payable              3
Cr Platform Fee Revenue           3
```

Merchant net settlement amount:

```text
97
```

## 5. Funding Example

Bank top-up NT$1,000:

```text
Dr Bank Funding Clearing       1,000
Cr Customer Wallet Liability   1,000
```

External bank credit confirmation must be idempotent.

## 6. Wallet Projection

Wallet may maintain:

```text
AvailableBalance
PendingBalance
Version
UpdatedAt
```

for operational performance.

But reconciliation must be able to recompute / verify the balance against ledger entries.

## 7. Concurrency

Wallet update must prevent:

```text
Balance = 100

Request A: spend 80
Request B: spend 80
```

from both succeeding.

V1 uses:

- SQL transaction
- RowVersion / optimistic concurrency
- Conditional update
- Retry only for safe, bounded concurrency conflicts

Redis distributed lock may be added as coordination optimization but is not the correctness boundary.

## 8. Points

Loyalty points use a separate ledger/model:

```text
LoyaltyAccount
PointTransaction
```

Do not mix TWD ledger and points ledger.
