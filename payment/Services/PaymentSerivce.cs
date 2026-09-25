namespace Api.Payment.Services;

using System.Globalization;

using Api.Payment.Interfaces;
using Api.Payment.DTOs.Entry;
using Api.Payment.DTOs.Return;
using Api.Payment.Models;
using Shared.DTOs.Result;

using MercadoPago.Client;
using MercadoPago.Client.Common;
using MercadoPago.Client.Order;
using MercadoPago.Error;
using MercadoPago.Http;
using MercadoPago.Resource.Order;

public class PaymentService : IPaymentInterface
{
    private readonly OrderClient _mpClient = new();

    public async Task<Result<PaymentChargeCreatedDto>> CreatePaymentCharge(
        EntryDataPayment dto)
    {
        try
        {
            const decimal amount = 150.00m;

            // Garante o formato decimal exigido pela API:
            // 110.00 em vez de 110,00
            var amountString = amount.ToString(
                "0.00",
                CultureInfo.InvariantCulture
            );

            var request = new OrderCreateRequest
            {
                Type = "online",

                TotalAmount = amountString,

                // Referência externa usada para relacionar
                // a Order do Mercado Pago com seu sistema.
                ExternalReference = $"consulta-{Guid.NewGuid():N}",

                ProcessingMode = "automatic",

                Payer = new OrderPayerRequest
                {
                    Email = "test_user_8449101681005017322@testuser.com"
                },

                Transactions = new OrderTransactionRequest
                {
                    Payments = new List<OrderPaymentRequest>
                    {
                        new OrderPaymentRequest
                        {
                            Amount = amountString,

                            PaymentMethod = new OrderPaymentMethodRequest
                            {
                                Id = "pix",
                                Type = "bank_transfer"
                            }
                        }
                    }
                }
            };

            // Idempotência:
            // impede que uma mesma requisição seja processada
            // duas vezes em caso de retry.
            var requestOptions = new RequestOptions();

            requestOptions.CustomHeaders.Add(
                Headers.IDEMPOTENCY_KEY,
                Guid.NewGuid().ToString()
            );

            // DEBUG TEMPORÁRIO
            Console.WriteLine(
                $"[Mercado Pago] TotalAmount: [{request.TotalAmount}]"
            );

            Console.WriteLine(
                $"[Mercado Pago] Amount: " +
                $"[{request.Transactions?.Payments?.FirstOrDefault()?.Amount}]"
            );

            Console.WriteLine(
                $"[Mercado Pago] ExternalReference: " +
                $"[{request.ExternalReference}]"
            );

            // Cria a Order no Mercado Pago
            Order order = await _mpClient.CreateAsync(
                request,
                requestOptions
            );

            // Recupera o primeiro pagamento da Order
            var payment = order.Transactions?
                .Payments?
                .FirstOrDefault();

            if (payment == null)
            {
                return new Result<PaymentChargeCreatedDto>
                {
                    Success = false,
                    Message =
                        "O Mercado Pago criou a Order, " +
                        "mas não retornou nenhum pagamento."
                };
            }

            var paymentMethod = payment.PaymentMethod;

            // Converte o valor retornado pelo Mercado Pago
            // novamente para decimal.
            decimal paymentAmount = amount;

            if (!string.IsNullOrWhiteSpace(payment.Amount) &&
                decimal.TryParse(
                    payment.Amount,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var parsedAmount))
            {
                paymentAmount = parsedAmount;
            }

            var chargeDto = new PaymentChargeCreatedDto
            {
                // O ID do pagamento da Orders API é string.
                OrderId = order.Id,
                PaymentChargeId = payment.Id,

                // Código PIX copia e cola
                QrCode = paymentMethod?.QrCode,

                // QR Code em Base64
                QrCodeBase64 = paymentMethod?.QrCodeBase64,

                Amount = paymentAmount,

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
                Message =
                    $"Erro ao criar cobrança: " +
                    $"{ex.ApiError?.Message ?? ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new Result<PaymentChargeCreatedDto>
            {
                Success = false,
                Message =
                    $"Erro interno ao criar cobrança: {ex.Message}"
            };
        }
    }

    private static PaymentChargeStatus MapStatus(string? mpStatus)
    {
        return mpStatus switch
        {
            "processed" =>
                PaymentChargeStatus.Paid,

            "action_required" =>
                PaymentChargeStatus.Pending,

            "pending" =>
                PaymentChargeStatus.Pending,

            "rejected" =>
                PaymentChargeStatus.Failed,

            "cancelled" =>
                PaymentChargeStatus.Cancelled,

            _ =>
                PaymentChargeStatus.Pending
        };
    }
    public async Task ProcessWebhook(string orderId)
{
    Console.WriteLine(
        $"[Mercado Pago] Consultando Order: {orderId}"
    );

    try
    {
        var order = await _mpClient.GetAsync(orderId);

        Console.WriteLine(
            $"[Mercado Pago] Order ID: {order.Id}"
        );

        Console.WriteLine(
            $"[Mercado Pago] Order Status: {order.Status}"
        );

        var payment = order.Transactions?
            .Payments?
            .FirstOrDefault();

        if (payment == null)
        {
            Console.WriteLine(
                "[Mercado Pago] Nenhum pagamento encontrado."
            );

            return;
        }

        Console.WriteLine(
            $"[Mercado Pago] Payment ID: {payment.Id}"
        );

        Console.WriteLine(
            $"[Mercado Pago] Payment Status: {payment.Status}"
        );
    }
    catch (MercadoPagoApiException ex)
    {
        Console.WriteLine(
            $"[Mercado Pago] Erro ao consultar Order: " +
            $"{ex.ApiError?.Message ?? ex.Message}"
        );

        throw;
    }
}
}
