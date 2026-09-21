using Audisoft.Application.Common;
using Audisoft.Application.Contracts.Repositories;
using Audisoft.Application.Contracts.Services;
using Audisoft.Application.Exceptions;
using Audisoft.Application.Mappers;
using Audisoft.Application.RequestFeatures;
using Audisoft.Application.Requests;
using FluentValidation;

namespace Audisoft.Application.Services;

public class NotaService : INotaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateNotaRequest> _createValidator;
    private readonly IValidator<UpdateNotaRequest> _updateValidator;

    public NotaService(
        IUnitOfWork unitOfWork,
        IValidator<CreateNotaRequest> createValidator,
        IValidator<UpdateNotaRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<PagedList<NotaRequest>> GetAllAsync(
        NotaFilterParams filterParams, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new NotaParameters
        {
            EstudianteId = filterParams.EstudianteId,
            ProfesorId = filterParams.ProfesorId,
            Materia = filterParams.Materia,
            ValorMinimo = filterParams.ValorMinimo,
            PageNumber = filterParams.PageNumber,
            PageSize = filterParams.PageSize
        };

        var notas = await _unitOfWork.NotaRepository.GetNotasAsync(
            parameters, trackChanges: false, cancellationToken);
        var count = await _unitOfWork.NotaRepository.GetNotasCountAsync(
            parameters, cancellationToken);

        var dtos = notas.Select(n => n.ToDto()).ToList();

        return new PagedList<NotaRequest>(dtos, count, filterParams.PageNumber, filterParams.PageSize);
    }

    public async Task<NotaRequest?> GetByIdAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var nota = await _unitOfWork.NotaRepository.GetNotaAsync(id, trackChanges: false, cancellationToken);

        return nota?.ToDto();
    }

    public async Task<NotaSmallRequest> CreateAsync(
        CreateNotaRequest dto,
        CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var nota = dto.ToEntity();
        _unitOfWork.NotaRepository.CreateNota(nota);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return nota.ToSmallDto();
    }

    public async Task UpdateAsync(
        int id, 
        UpdateNotaRequest dto, 
        CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var nota = await _unitOfWork.NotaRepository.GetNotaAsync(id, trackChanges: true, cancellationToken)
            ?? throw new NotFoundException($"Nota con id {id} no encontrada.");

        nota.ApplyUpdate(dto);
        _unitOfWork.NotaRepository.UpdateNota(nota);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var nota = await _unitOfWork.NotaRepository.GetNotaAsync(id, trackChanges: true, cancellationToken)
            ?? throw new NotFoundException($"Nota con id {id} no encontrada.");

        var estudianteInactivo = 
            await _unitOfWork.EstudianteRepository.IsEstudianteInactivoAsync(nota.EstudianteId, 
            cancellationToken);

        var profesorInactivo = 
            await _unitOfWork.ProfesorRepository.IsProfesorInactivoAsync(nota.ProfesorId, 
            cancellationToken);

        if (!estudianteInactivo || !profesorInactivo)
        {
            throw new Exception("Solo se puede eliminar la nota si el estudiante y el profesor están inactivos.");
        }

        _unitOfWork.NotaRepository.DeleteNota(nota);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}