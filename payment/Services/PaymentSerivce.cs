namespace Api.Payment.Services;
using Api.Payment.Interfaces;
using Api.Payment.DTOs.Entry;
using Api.Payment.DTOs.Return;
using Api.Payment.Models;
using Shared.DTOs.Result;
using MercadoPago.Client;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Error;
using MercadoPago.Http;
using MercadoPago.Resource.Payment;

public class PaymentService : IPaymentInterface
{
    private readonly PaymentClient _mpClient = new();

    public async Task<Result<PaymentChargeCreatedDto>> CreatePaymentCharge(EntryDataPayment dto)
    {
        try
        {
            var request = new PaymentCreateRequest
            {
                TransactionAmount = 110.00m,
                Description = $"Consulta 12/02",
                PaymentMethodId = "pix",
                Payer = new PaymentPayerRequest
                {
                    Email = "test_user_8449101681005017322@testuser.com",
                    FirstName = "TESTUSER8449101681005017322",
                    Identification = new IdentificationRequest
                    {
                        Type = "CPF",
                        Number = "41270798863"
                    }
                },
                ExternalReference = "Andre Carlos",
                NotificationUrl = "https://6qpvvrwf-5095.brs.devtunnels.ms/api/payment/webhook" // troca depois
            };

            var requestOptions = new RequestOptions();
            requestOptions.CustomHeaders.Add(Headers.IDEMPOTENCY_KEY, Guid.NewGuid().ToString());

            Payment payment = await _mpClient.CreateAsync(request, requestOptions);

            var chargeDto = new PaymentChargeCreatedDto
            {
                PaymentChargeId = payment.Id ?? 0,
                QrCode = payment.PointOfInteraction?.TransactionData?.QrCode,
                QrCodeBase64 = payment.PointOfInteraction?.TransactionData?.QrCodeBase64,
                Amount = payment.TransactionAmount ?? 111.00m,
                Status = MapStatus(payment.Status)
            };

            return new Result<PaymentChargeCreatedDto>
            {
                Success = true,
                Data = chargeDto,
                Message = "Cobrança Pix criada com sucesso."
            };
        }
        catch (MercadoPagoApiException ex)
        {
            return new Result<PaymentChargeCreatedDto>
            {
                Success = false,
                Message = $"Erro ao criar cobrança: {ex.ApiError?.Message ?? ex.Message}"
            };
        }
    }

    private static PaymentChargeStatus MapStatus(string? mpStatus) => mpStatus switch
    {
        "approved" => PaymentChargeStatus.Paid,
        "pending" or "in_process" => PaymentChargeStatus.Pending,
        "rejected" => PaymentChargeStatus.Failed,
        "cancelled" => PaymentChargeStatus.Cancelled,
        _ => PaymentChargeStatus.Pending
    };
}