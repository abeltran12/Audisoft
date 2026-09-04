using Audisoft.Application.Contracts.Services;
using Audisoft.Application.RequestFeatures;
using Audisoft.Application.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Audisoft.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudiantesController : ControllerBase
{
    private readonly IEstudianteService _estudianteService;

    public EstudiantesController(IEstudianteService estudianteService)
    {
        _estudianteService = estudianteService;
    }

    /// <summary>
    /// Obtiene un listado paginado de estudiantes, con filtro opcional por nombre.
    /// </summary>
    /// <param name="parameters">Parámetros de filtro y paginación.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>Lista paginada de estudiantes.</returns>
    /// <response code="200">Listado obtenido correctamente.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstudianteRequest>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EstudianteRequest>>> GetEstudiantes(
        [FromQuery] EstudianteParameters parameters,
        CancellationToken cancellationToken)
    {
        var estudiantes = await _estudianteService.GetAllAsync(parameters, cancellationToken);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(estudiantes.MetaData));

        return Ok(estudiantes);
    }

    /// <summary>
    /// Obtiene un estudiante por su id.
    /// </summary>
    /// <param name="id">Id del estudiante a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>El estudiante encontrado.</returns>
    /// <response code="200">Estudiante encontrado y devuelto correctamente.</response>
    /// <response code="404">No existe un estudiante con el id indicado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EstudianteRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EstudianteRequest>> GetEstudiante(
        int id,
        CancellationToken cancellationToken)
    {
        var estudiante = await _estudianteService.GetByIdAsync(id, cancellationToken);

        if (estudiante == null)
            return NotFound();

        return Ok(estudiante);
    }

    /// <summary>
    /// Crea un nuevo estudiante.
    /// </summary>
    /// <param name="dto">Datos del estudiante a crear.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>El estudiante creado.</returns>
    /// <response code="201">Estudiante creado correctamente.</response>
    /// <response code="400">Los datos enviados no son válidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(EstudianteRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EstudianteRequest>> CreateEstudiante(
        CreateEstudianteRequest dto,
        CancellationToken cancellationToken)
    {
        var estudiante = await _estudianteService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.Id }, estudiante);
    }

    /// <summary>
    /// Actualiza un estudiante existente.
    /// </summary>
    /// <param name="id">Id del estudiante a actualizar.</param>
    /// <param name="dto">Nuevos datos del estudiante.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <response code="204">Estudiante actualizado correctamente.</response>
    /// <response code="400">Los datos enviados no son válidos.</response>
    /// <response code="404">No existe un estudiante con el id indicado.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEstudiante(
        int id,
        UpdateEstudianteRequest dto,
        CancellationToken cancellationToken)
    {
        await _estudianteService.UpdateAsync(id, dto, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Elimina un estudiante existente.
    /// </summary>
    /// <param name="id">Id del estudiante a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <response code="204">Estudiante eliminado correctamente.</response>
    /// <response code="404">No existe un estudiante con el id indicado.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEstudiante(
        int id,
        CancellationToken cancellationToken)
    {
        await _estudianteService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}