# 07. Frontend & UX Scope

## 1. Goal

Frontend exists to demonstrate backend behavior. It should remain thin and avoid dominating project complexity.

Recommended stack:

- Next.js
- TypeScript
- Tailwind CSS
- shadcn/ui
- PWA where useful

## 2. Consumer PWA

Primary screens:

### Home

- Wallet balance
- Scan to pay
- Payment code
- Top-up
- Bill payment
- Points
- Recent transactions

### Wallet

- Available balance
- Funding history
- Payment history

### Payment

- QR scanner
- Payment confirmation
- Processing
- Succeeded
- Failed / Unknown

### Funding

- Bank top-up
- Crypto funding entry (V1.5)

### Bill Payment

Buttons:

- Water
- Electricity
- Gas
- Telecom
- Parking
- Building management fee
- Tuition / other biller simulator

### Loyalty

- Points balance
- Earn history
- Redeem history

## 3. Merchant POS PWA

Primary flows:

### Amount Entry

```text
Amount
[Collect Payment]
```

### Merchant-Presented QR

Generate QR for consumer scan.

### Consumer-Presented QR

Use browser camera to scan consumer one-time token.

### Result

Show:

- amount
- status
- order id
- payment id
- processing / succeeded / failed

### Additional

- transaction lookup
- refund
- receipt representation

## 4. Merchant Admin

Recommended navigation:

```text
Dashboard
Transactions
Refunds
Settlement
  ├─ Batches
  └─ Statements
Stores
Terminals
Reports
API Keys
Webhooks
```

Dashboard metrics:

- today's GMV
- payment count
- success rate
- refunds
- pending settlement
- settled amount

## 5. Platform Ops Console

Later V1 / V1.5:

```text
Payment Search
Unknown Transactions
Reconciliation
Settlement Failures
Webhook Delivery
Dead Letter
Audit Log
Simulator Controls
```

Simulator Controls may intentionally trigger:

- provider timeout
- decline
- slow response
- 500
- duplicate callback

This makes failure scenarios directly demoable.

## 6. Frontend Rule

Frontend must not independently derive authoritative payment state.

Authoritative state comes from backend APIs/events.
