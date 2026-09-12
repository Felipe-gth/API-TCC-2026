namespace Api.Payment.Interfaces;
using Shared.DTOs.Result;
using Api.Payment.DTOs.Entry;
using Api.Payment.DTOs.Return;
public interface IPaymentInterface
{
    Task<Result<PaymentChargeCreatedDto>> CreatePaymentCharge(EntryDataPayment dto);
}