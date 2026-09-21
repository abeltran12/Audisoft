using Audisoft.Web.Models;
using Audisoft.Web.Models.Abstractions;
using Audisoft.Web.Models.Parameters;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Audisoft.Web.Services;

public class EstudianteApiService : IEstudianteApiService
{
    private readonly HttpClient _httpClient;

    public EstudianteApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedList<EstudianteRequest>> GetEstudiantesAsync(
        EstudianteParameters parameters, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = $"api/estudiantes?PageNumber={parameters.PageNumber}&PageSize={parameters.PageSize}";

            if (!string.IsNullOrEmpty(parameters.Nombre))
                query += $"&Nombre={Uri.EscapeDataString(parameters.Nombre)}";

            if (parameters.Id.HasValue)
                query += $"&Id={parameters.Id.Value}";

            if (parameters.SoloInactivos)
                query += "&SoloInactivos=true";

            var response = await _httpClient.GetAsync(query, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);

            var estudiantes = 
                await response.Content.ReadFromJsonAsync<List<EstudianteRequest>>
                    (cancellationToken: cancellationToken)
                ?? new List<EstudianteRequest>();

            var metaData = new MetaData();
            if (response.Headers.TryGetValues("X-Pagination", out var values))
            {
                var json = values.FirstOrDefault();
                if (json != null)
                    metaData = JsonSerializer.Deserialize<MetaData>(json) ?? new MetaData();
            }

            return new PagedList<EstudianteRequest>(estudiantes, metaData);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al obtener estudiantes: {ex.Message}");
        }
    }

    public async Task<EstudianteRequest?> GetEstudianteAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/estudiantes/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            await ThrowIfErrorAsync(response, cancellationToken);

            return await response.Content.ReadFromJsonAsync<EstudianteRequest>
                (cancellationToken: cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al obtener el estudiante: {ex.Message}");
        }
    }

    public async Task<EstudianteRequest> CreateEstudianteAsync(CreateEstudianteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/estudiantes", request, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);

            return await response.Content.ReadFromJsonAsync<EstudianteRequest>
                (cancellationToken: cancellationToken)
                ?? throw new ApiException(0, "La API no devolvió el estudiante creado.");
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al crear el estudiante: {ex.Message}");
        }
    }

    public async Task UpdateEstudianteAsync(int id, UpdateEstudianteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/estudiantes/{id}", request, cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al actualizar el estudiante: {ex.Message}");
        }
    }

    public async Task DeleteEstudianteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/estudiantes/{id}", cancellationToken);
            await ThrowIfErrorAsync(response, cancellationToken);
        }
        catch (ApiException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException(0, $"Error de conexión al eliminar el estudiante: {ex.Message}");
        }
    }

    private static async Task ThrowIfErrorAsync(
        HttpResponseMessage response, 
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var problemDetails = 
            await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);

        var message = problemDetails?.Detail
            ?? problemDetails?.Title
            ?? $"Error inesperado ({(int)response.StatusCode})";

        throw new ApiException((int)response.StatusCode, message);
    }
}
