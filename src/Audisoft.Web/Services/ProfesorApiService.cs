using Audisoft.Web.Models;
using Audisoft.Web.Models.Abstractions;
using Audisoft.Web.Models.Parameters;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Audisoft.Web.Services;

public class ProfesorApiService : IProfesorApiService
{
    private readonly HttpClient _httpClient;

    public ProfesorApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedList<ProfesorRequest>> GetProfesoresAsync(
        ProfesorParameters parameters, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = $"api/profesores?PageNumber={parameters.PageNumber}&PageSize={parameters.PageSize}";

            if (!string.IsNullOrEmpty(parameters.Nombre))
                query += $"&Nombre={Uri.EscapeDataString(parameters.Nombre)}";

            if (parameters.Id.HasValue)
                query += $"&Id={parameters.Id.Value}";

            if (parameters.SoloInactivos)
                query += "&SoloInactivos=true";

            var response = await _httpClient.GetAsync(query, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);

            var profesores = await response.Content.ReadFromJsonAsync<List<ProfesorRequest>>
                (cancellationToken: cancellationToken)
                ?? new List<ProfesorRequest>();

            var metaData = new MetaData();
            if (response.Headers.TryGetValues("X-Pagination", out var values))
            {
                var json = values.FirstOrDefault();
                if (json != null)
                    metaData = JsonSerializer.Deserialize<MetaData>(json) ?? new MetaData();
            }

            return new PagedList<ProfesorRequest>(profesores, metaData);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al obtener profesores: {ex.Message}");
        }
    }

    public async Task<ProfesorRequest?> GetProfesorAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/profesores/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            await ThrowIfErrorAsync(response, cancellationToken);

            return await response.Content.ReadFromJsonAsync<ProfesorRequest>(cancellationToken: cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al obtener el profesor: {ex.Message}");
        }
    }

    public async Task<ProfesorRequest> CreateProfesorAsync(
        CreateProfesorRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/profesores", request, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);

            return await response.Content.ReadFromJsonAsync<ProfesorRequest>
                (cancellationToken: cancellationToken)
                ?? throw new ApiException(0, "La API no devolvió el profesor creado.");
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al crear el profesor: {ex.Message}");
        }
    }

    public async Task UpdateProfesorAsync(
        int id, 
        UpdateProfesorRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/profesores/{id}", request, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al actualizar el profesor: {ex.Message}");
        }
    }

    public async Task DeleteProfesorAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/profesores/{id}", cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al eliminar el profesor: {ex.Message}");
        }
    }

    private static async Task ThrowIfErrorAsync(
        HttpResponseMessage response, 
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>
            (cancellationToken: cancellationToken);

        var message = problemDetails?.Detail
            ?? problemDetails?.Title
            ?? $"Error inesperado ({(int)response.StatusCode})";

        throw new ApiException((int)response.StatusCode, message);
    }
}
