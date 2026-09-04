using Audisoft.Application.Common;
using Audisoft.Application.Contracts.Repositories;
using Audisoft.Application.Contracts.Services;
using Audisoft.Application.Exceptions;
using Audisoft.Application.Mappers;
using Audisoft.Application.RequestFeatures;
using Audisoft.Application.Requests;
using FluentValidation;

namespace Audisoft.Application.Services;

public class ProfesorService : IProfesorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProfesorRequest> _createValidator;
    private readonly IValidator<UpdateProfesorRequest> _updateValidator;

    public ProfesorService(
        IUnitOfWork unitOfWork,
        IValidator<CreateProfesorRequest> createValidator,
        IValidator<UpdateProfesorRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<PagedList<ProfesorRequest>> GetAllAsync(
        ProfesorParameters parameters, 
        CancellationToken cancellationToken = default)
    {
        var profesores = await _unitOfWork.ProfesorRepository.GetProfesoresAsync
            (parameters, trackChanges: false, cancellationToken);

        var count = await _unitOfWork.ProfesorRepository.GetProfesoresCountAsync
            (parameters, cancellationToken);

        var dtos = profesores.Select(p => p.ToDto()).ToList();

        return new PagedList<ProfesorRequest>(dtos, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<ProfesorRequest?> GetByIdAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var profesor = await _unitOfWork.ProfesorRepository.GetProfesorAsync
            (id, trackChanges: false, cancellationToken);
        
        return profesor?.ToDto();
    }

    public async Task<ProfesorRequest> CreateAsync(
        CreateProfesorRequest dto, 
        CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var profesor = dto.ToEntity();
        _unitOfWork.ProfesorRepository.CreateProfesor(profesor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return profesor.ToDto();
    }

    public async Task UpdateAsync(
        int id, 
        UpdateProfesorRequest dto, 
        CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var profesor = await _unitOfWork.ProfesorRepository.GetProfesorAsync
            (id, trackChanges: true, cancellationToken)
            ?? throw new NotFoundException($"Profesor con id {id} no encontrado.");

        profesor.ApplyUpdate(dto);
        _unitOfWork.ProfesorRepository.UpdateProfesor(profesor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var profesor = await _unitOfWork.ProfesorRepository.GetProfesorAsync
            (id, trackChanges: true, cancellationToken)
            ?? throw new NotFoundException($"Profesor con id {id} no encontrado.");

        var tieneNotasActivas = await _unitOfWork.NotaRepository.
            HasActiveNotasByProfesorAsync(id, cancellationToken);

        if (tieneNotasActivas)
            throw new BusinessRuleException("No se puede eliminar el profesor porque tiene notas activas asociadas.");

        _unitOfWork.ProfesorRepository.DeleteProfesor(profesor);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}