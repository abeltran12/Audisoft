using Audisoft.Application.Common;
using Audisoft.Application.Contracts.Services;
using Audisoft.Application.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Audisoft.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotasController : ControllerBase
{
    private readonly INotaService _notaService;

    public NotasController(INotaService notaService)
    {
        _notaService = notaService;
    }

    /// <summary>
    /// Obtiene un listado paginado de notas, con filtro opcional por estudiante, profesor, materia y valor mínimo.
    /// </summary>
    /// <param name="filterParams">Parámetros de filtro y paginación.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>Lista paginada de notas.</returns>
    /// <response code="200">Listado obtenido correctamente.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotaRequest>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NotaRequest>>> GetNotas(
        [FromQuery] NotaFilterParams filterParams,
        CancellationToken cancellationToken)
    {
        var notas = await _notaService.GetAllAsync(filterParams, cancellationToken);

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(notas.MetaData));

        return Ok(notas);
    }

    /// <summary>
    /// Obtiene una nota por su id.
    /// </summary>
    /// <param name="id">Id de la nota a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>La nota encontrada.</returns>
    /// <response code="200">Nota encontrada y devuelta correctamente.</response>
    /// <response code="404">No existe una nota con el id indicado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(NotaRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotaRequest>> GetNota(
        int id,
        CancellationToken cancellationToken)
    {
        var nota = await _notaService.GetByIdAsync(id, cancellationToken);

        if (nota == null)
            return NotFound();

        return Ok(nota);
    }

    /// <summary>
    /// Crea una nueva nota.
    /// </summary>
    /// <param name="dto">Datos de la nota a crear.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <returns>La nota creada.</returns>
    /// <response code="201">Nota creada correctamente.</response>
    /// <response code="400">Los datos enviados no son válidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(NotaRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotaRequest>> CreateNota(
        CreateNotaRequest dto,
        CancellationToken cancellationToken)
    {
        var nota = await _notaService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(nameof(GetNota), new { id = nota.Id }, nota);
    }

    /// <summary>
    /// Actualiza una nota existente.
    /// </summary>
    /// <param name="id">Id de la nota a actualizar.</param>
    /// <param name="dto">Nuevos datos de la nota.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <response code="204">Nota actualizada correctamente.</response>
    /// <response code="400">Los datos enviados no son válidos.</response>
    /// <response code="404">No existe una nota con el id indicado.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateNota(
        int id,
        UpdateNotaRequest dto,
        CancellationToken cancellationToken)
    {
        await _notaService.UpdateAsync(id, dto, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Elimina una nota existente.
    /// </summary>
    /// <param name="id">Id de la nota a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación de la petición.</param>
    /// <response code="204">Nota eliminada correctamente.</response>
    /// <response code="404">No existe una nota con el id indicado.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNota(
        int id,
        CancellationToken cancellationToken)
    {
        await _notaService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
