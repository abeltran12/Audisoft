using Audisoft.Web.Models;
using Audisoft.Web.Models.Abstractions;
using Audisoft.Web.Models.Parameters;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Audisoft.Web.Services;

public class NotaApiService : INotaApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public NotaApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedList<NotaRequest>> GetNotasAsync(
        NotaFilterParams filterParams, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = $"api/notas?PageNumber={filterParams.PageNumber}&PageSize={filterParams.PageSize}";

            if (filterParams.EstudianteId.HasValue)
                query += $"&EstudianteId={filterParams.EstudianteId}";

            if (filterParams.ProfesorId.HasValue)
                query += $"&ProfesorId={filterParams.ProfesorId}";

            if (filterParams.Materia.HasValue)
                query += $"&Materia={Uri.EscapeDataString(filterParams.Materia.Value.ToString())}";

            if (filterParams.ValorMinimo.HasValue)
                query += $"&ValorMinimo={filterParams.ValorMinimo}";

            if (filterParams.SoloInactivos)
                query += "&SoloInactivos=true";

            var response = await _httpClient.GetAsync(query, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);

            var notas = await response.Content.ReadFromJsonAsync<List<NotaRequest>>
                (SerializerOptions, cancellationToken)
                ?? [];

            var metaData = new MetaData();
            if (response.Headers.TryGetValues("X-Pagination", out var values))
            {
                var json = values.FirstOrDefault();
                if (json != null)
                    metaData = JsonSerializer.Deserialize<MetaData>(json) ?? new MetaData();
            }

            return new PagedList<NotaRequest>(notas, metaData);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al obtener notas: {ex.Message}");
        }
    }

    public async Task<NotaRequest?> GetNotaAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/notas/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            await ThrowIfErrorAsync(response, cancellationToken);

            return await response.Content.ReadFromJsonAsync<NotaRequest>(SerializerOptions, cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al obtener la nota: {ex.Message}");
        }
    }

    public async Task<NotaRequest> CreateNotaAsync(
        CreateNotaRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/notas", request, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);

            return await response.Content.ReadFromJsonAsync<NotaRequest>(SerializerOptions, cancellationToken)
                ?? throw new ApiException(0, "La API no devolvió la nota creada.");
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al crear la nota: {ex.Message}");
        }
    }

    public async Task UpdateNotaAsync(
        int id, 
        UpdateNotaRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/notas/{id}", request, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al actualizar la nota: {ex.Message}");
        }
    }

    public async Task DeleteNotaAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/notas/{id}", cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al eliminar la nota: {ex.Message}");
        }
    }

    private static async Task ThrowIfErrorAsync(
        HttpResponseMessage response, 
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);

        var message = problemDetails?.Detail
            ?? problemDetails?.Title
            ?? $"Error inesperado ({(int)response.StatusCode})";

        throw new ApiException((int)response.StatusCode, message);
    }
}
