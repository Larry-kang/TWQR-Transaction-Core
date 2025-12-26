using Microsoft.AspNetCore.Mvc;
using TWQR.Application.DTOs;
using TWQR.Domain.Entities;
using TWQR.Domain.Interfaces;

namespace TWQR.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionRepository _repository;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ITransactionRepository repository, ILogger<PaymentController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
            [FromBody] CreatePaymentRequest request)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                return BadRequest("Idempotency-Key header is required.");
            }

            var existingTransaction = await _repository.GetByIdempotencyKeyAsync(idempotencyKey);
            if (existingTransaction != null)
            {
                // In a real idempotency implementation, we might return the original result
                // For now, checking existence and returning conflict or just the existing status
                return Conflict($"Transaction with Idempotency-Key '{idempotencyKey}' already exists.");
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                IdempotencyKey = idempotencyKey,
                Amount = request.Amount,
                Currency = "TWD",
                Status = TransactionStatus.Created, // Assuming Created is part of the enum or using Pending
                CreatedAt = DateTime.UtcNow
            };
            
            // Note: User prompt said "Status Created", but I defined: Pending, Processing, Completed, Failed, Cancelled
            // I will use 'Pending' as 'Created' equivalent if 'Created' doesn't exist, or update the Enum.
            // Let's check the Enum definition I made earlier.
            // Earlier I wrote: Pending, Processing, Completed, Failed, Cancelled.
            // I should use "Pending" or update the Enum. The user specifically asked for "Created".
            // To respect the user prompt strictly, I should likely update the Enum or use Pending if Created is unavailable.
            // I'll stick to 'Pending' for now to match the code I just wrote, OR better yet, let's update the Enum definition to include Created, or just use Pending and comment.
            // Actually, the user's prompt in step 3 says: "«Ø¥ß Transaction Entity (ª¬ºA Created)". 
            // I should probably ensure the Enum has "Created" to be safe.
            // I'll stick with Pending as the logical equivalent for now to avoid re-editing the Entity unless compiling fails or I decide to do a quick fix. 
            // Wait, I can define it as TransactionStatus.Pending and maybe the user meant logically created. 
            // Let's assume TransactionStatus.Pending is fine, or I will update the Enum file.
            // I will update the Enum file to be safe. It helps to be precise.
            
            transaction.Status = TransactionStatus.Created; 

            await _repository.AddAsync(transaction);

            return Ok(new { TransactionId = transaction.Id, Status = transaction.Status });
        }
    }
}
