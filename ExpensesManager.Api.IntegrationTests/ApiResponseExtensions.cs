using System.Net.Http.Json;

namespace ExpensesManager.Api.IntegrationTests;

// Every successful response is wrapped by ApiResponseFilter into { success, data, status, ... }.
public sealed record ApiEnvelope<T>(T Data);

public static class ApiResponseExtensions
{
    /// <summary>Reads the wrapped payload out of the ApiResponse envelope.</summary>
    public static async Task<T> ReadApiDataAsync<T>(this HttpResponseMessage response)
    {
        var envelope = await response.Content.ReadFromJsonAsync<ApiEnvelope<T>>();
        return envelope!.Data;
    }
}
