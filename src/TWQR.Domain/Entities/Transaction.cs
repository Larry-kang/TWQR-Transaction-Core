using System;

namespace TWQR.Domain.Entities
{
    public enum TransactionStatus
    {
        Created,
        Pending,
        Processing,
        Completed,
        Failed,
        Cancelled
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public string IdempotencyKey { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "TWD";
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
