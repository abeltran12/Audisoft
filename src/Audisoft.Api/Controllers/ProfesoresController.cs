using Audisoft.Application.Contracts.Services;
using Audisoft.Application.RequestFeatures;
using Audisoft.Application.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Audisoft.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfesoresController : ControllerBase
{
    private readonly IProfesorService _profesorService;

    public ProfesoresController(IProfesorService profesorService)
    {
        _profesorService = profesorService;
    }

    /// <summary>
    /// Obtiene un listado paginado de profesores, con filtro opcional por nombre.
    /// </summary>
    /// <param name="parameters">Parámetros de filtro y paginación.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>Lista paginada de profesores.</returns>
    /// <response code="200">Listado obtenido correctamente.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProfesorRequest>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProfesorRequest>>> GetProfesores(
        [FromQuery] ProfesorParameters parameters,
        CancellationToken cancellationToken)
    {
        var profesores = await _profesorService.GetAllAsync(parameters, cancellationToken);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(profesores.MetaData));

        return Ok(profesores);
    }

    /// <summary>
    /// Obtiene un profesor por su id.
    /// </summary>
    /// <param name="id">Id del profesor a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>El profesor encontrado.</returns>
    /// <response code="200">Profesor encontrado y devuelto correctamente.</response>
    /// <response code="404">No existe un profesor con el id indicado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProfesorRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfesorRequest>> GetProfesor(
        int id,
        CancellationToken cancellationToken)
    {
        var profesor = await _profesorService.GetByIdAsync(id, cancellationToken);

        if (profesor == null)
            return NotFound();

        return Ok(profesor);
    }

    /// <summary>
    /// Crea un nuevo profesor.
    /// </summary>
    /// <param name="dto">Datos del profesor a crear.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>El profesor creado.</returns>
    /// <response code="201">Profesor creado correctamente.</response>
    /// <response code="400">Los datos enviados no son válidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProfesorRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfesorRequest>> CreateProfesor(
        CreateProfesorRequest dto,
        CancellationToken cancellationToken)
    {
        var profesor = await _profesorService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetProfesor), new { id = profesor.Id }, profesor);
    }

    /// <summary>
    /// Actualiza un profesor existente.
    /// </summary>
    /// <param name="id">Id del profesor a actualizar.</param>
    /// <param name="dto">Nuevos datos del profesor.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <response code="204">Profesor actualizado correctamente.</response>
    /// <response code="400">Los datos enviados no son válidos.</response>
    /// <response code="404">No existe un profesor con el id indicado.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfesor(
        int id,
        UpdateProfesorRequest dto,
        CancellationToken cancellationToken)
    {
        await _profesorService.UpdateAsync(id, dto, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Elimina un profesor existente.
    /// </summary>
    /// <param name="id">Id del profesor a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <response code="204">Profesor eliminado correctamente.</response>
    /// <response code="404">No existe un profesor con el id indicado.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfesor(
        int id,
        CancellationToken cancellationToken)
    {
        await _profesorService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
