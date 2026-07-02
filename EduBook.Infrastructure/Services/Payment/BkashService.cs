using EduBook.Application.Common;
using EduBook.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace EduBook.Infrastructure.Services.Payment;

public class BkashService : IBkashService
{
    private readonly BkashSettings _settings;
    private readonly HttpClient _httpClient;
    private string? _cachedToken;
    private DateTime _tokenExpiry;

    public BkashService(IOptions<BkashSettings> settings, HttpClient httpClient)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
    }

    public async Task<string> GetTokenAsync()
    {
        if (_cachedToken != null && DateTime.UtcNow < _tokenExpiry)
            return _cachedToken;

        var request = new
        {
            app_key = _settings.AppKey,
            app_secret = _settings.AppSecret
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("username", _settings.Username);
        _httpClient.DefaultRequestHeaders.Add("password", _settings.Password);

        var response = await _httpClient.PostAsync(
            $"{_settings.BaseUrl}/tokenized/checkout/token/grant",
            content);

        var responseString = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

        _cachedToken = tokenResponse.GetProperty("id_token").GetString();
        _tokenExpiry = DateTime.UtcNow.AddHours(1);

        return _cachedToken!;
    }

    public async Task<BkashPaymentResponse> CreatePaymentAsync(decimal amount, string merchantInvoiceNumber)
    {
        var token = await GetTokenAsync();

        var request = new
        {
            mode = "0011",
            payerReference = merchantInvoiceNumber,
           // callbackURL = "https://yourdomain.com/api/v1/payments/bkash/callback",
            callbackURL = "https://localhost:7001/api/v1/payments/bkash/callback",
            amount = amount.ToString("F2"),
            currency = "BDT",
            intent = "sale",
            merchantInvoiceNumber
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", token);
        _httpClient.DefaultRequestHeaders.Add("X-APP-Key", _settings.AppKey);

        var response = await _httpClient.PostAsync(
            $"{_settings.BaseUrl}/tokenized/checkout/create",
            content);

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseString);

        return new BkashPaymentResponse(
            result.GetProperty("paymentID").GetString()!,
            result.GetProperty("bkashURL").GetString()!,
            result.GetProperty("statusCode").GetString()!,
            null
        );
    }

    public async Task<BkashPaymentResponse> ExecutePaymentAsync(string paymentId)
    {
        var token = await GetTokenAsync();

        var request = new { paymentID = paymentId };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", token);
        _httpClient.DefaultRequestHeaders.Add("X-APP-Key", _settings.AppKey);

        var response = await _httpClient.PostAsync(
            $"{_settings.BaseUrl}/tokenized/checkout/execute",
            content);

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseString);

        return new BkashPaymentResponse(
            result.GetProperty("paymentID").GetString()!,
            string.Empty,
            result.GetProperty("statusCode").GetString()!,
            result.TryGetProperty("trxID", out var trxId) ? trxId.GetString() : null
        );
    }

    public async Task<BkashQueryResponse> QueryPaymentAsync(string paymentId)
    {
        var token = await GetTokenAsync();

        var request = new { paymentID = paymentId };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", token);
        _httpClient.DefaultRequestHeaders.Add("X-APP-Key", _settings.AppKey);

        var response = await _httpClient.PostAsync(
            $"{_settings.BaseUrl}/tokenized/checkout/payment/status",
            content);

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(responseString);

        return new BkashQueryResponse(
            result.GetProperty("paymentID").GetString()!,
            result.GetProperty("trxID").GetString()!,
            result.GetProperty("transactionStatus").GetString()!,
            decimal.Parse(result.GetProperty("amount").GetString()!)
        );
    }
}