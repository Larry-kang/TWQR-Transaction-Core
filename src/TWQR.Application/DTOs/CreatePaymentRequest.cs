namespace TWQR.Application.DTOs
{
    public record CreatePaymentRequest(string MerchantId, decimal Amount, string OrderId);
}
