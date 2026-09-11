# 06. Settlement & Reconciliation

## 1. Merchant Clearing

Successful payment creates merchant receivable.

Example:

```text
Gross payment       100
Platform fee          3
Merchant net         97
```

Clearing determines the amount eligible for settlement.

## 2. Settlement Model

Suggested entities:

```text
SettlementBatch
SettlementItem
SettlementTransfer
```

Settlement states:

```text
Created
Calculating
Ready
Processing
Settled
Failed
PartiallySettled
```

Settlement item references original business transactions and must be traceable to ledger postings.

## 3. Settlement Batch

Example daily flow:

```text
Cutoff
  ↓
Select eligible merchant receivables
  ↓
Calculate gross / fees / refunds / net
  ↓
Create SettlementBatch
  ↓
Post settlement ledger movement
  ↓
Bank Transfer Simulator
  ↓
Settled / Failed / Unknown
```

The same eligible transaction must not enter two settlement batches.

## 4. Settlement Accounting Example

Merchant net payout NT$97:

```text
Dr Merchant Payable        97
Cr Settlement Clearing     97
```

After bank transfer confirmation, clearing is resolved according to the final accounting model.

Exact chart of accounts is implementation-defined but all movements must remain double-entry balanced.

## 5. Refund Effect

Refund before cutoff:

- Reduce receivable / settlement amount.

Refund after settlement:

- Create new compensating liability / debit for a future settlement or separate recovery process.

Never mutate the original settled transaction.

## 6. Reconciliation Purpose

Reconciliation resolves differences between:

- Platform payment state
- Ledger
- External provider
- Settlement
- Funding

## 7. Reconciliation Types

### Payment Reconciliation

Used for:

```text
Processing / Unknown
→ query external provider
→ resolve Succeeded / Failed
```

### Ledger Reconciliation

Validate:

- Business transaction has expected posting.
- Debit equals credit.
- No duplicate business reference posting.

### Settlement Reconciliation

Compare:

- Eligible transactions
- Batch items
- Bank transfer simulation result
- Ledger movement

### Funding Reconciliation

Compare:

- Bank / crypto external confirmation
- Funding order
- Wallet credit
- Ledger posting

## 8. Unknown Recovery

Example:

```text
Card authorization sent
Provider processed
Response lost
Payment = Unknown
       ↓
Reconciliation worker
       ↓
Provider inquiry
  ┌────┴────┐
  ▼         ▼
Success    Failed
  │         │
Resolve    Resolve
```

Unknown records require:

- next inquiry time
- inquiry count
- last provider status
- last error
- manual review eligibility

## 9. Ops Requirements

Platform Ops should be able to inspect:

- Unknown transactions
- Reconciliation attempts
- Settlement failures
- Provider mismatches
- Duplicate-risk cases
- Manual intervention history

Manual correction must be auditable and preferably represented as a new compensating action.
