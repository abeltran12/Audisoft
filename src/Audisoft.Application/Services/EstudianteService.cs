using Audisoft.Application.Common;
using Audisoft.Application.Contracts.Repositories;
using Audisoft.Application.Contracts.Services;
using Audisoft.Application.Exceptions;
using Audisoft.Application.Mappers;
using Audisoft.Application.RequestFeatures;
using Audisoft.Application.Requests;
using FluentValidation;

namespace Audisoft.Application.Services;

public class EstudianteService : IEstudianteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateEstudianteRequest> _createValidator;
    private readonly IValidator<UpdateEstudianteRequest> _updateValidator;

    public EstudianteService(
        IUnitOfWork unitOfWork,
        IValidator<CreateEstudianteRequest> createValidator,
        IValidator<UpdateEstudianteRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<PagedList<EstudianteRequest>> GetAllAsync(
        EstudianteParameters parameters, 
        CancellationToken cancellationToken = default)
    {
        var estudiantes = await _unitOfWork.EstudianteRepository.GetEstudiantesAsync
                (parameters, trackChanges: false, cancellationToken);

        var count = await _unitOfWork.EstudianteRepository.GetEstudiantesCountAsync
                (parameters, cancellationToken);

        var dtos = estudiantes.Select(e => e.ToDto()).ToList();

        return new PagedList<EstudianteRequest>(dtos, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<EstudianteRequest?> GetByIdAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var estudiante = await _unitOfWork.EstudianteRepository.GetEstudianteAsync
            (id, trackChanges: false, cancellationToken);

        return estudiante?.ToDto();
    }

    public async Task<EstudianteRequest> CreateAsync(
        CreateEstudianteRequest dto, 
        CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var estudiante = dto.ToEntity();

        _unitOfWork.EstudianteRepository.CreateEstudiante(estudiante);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return estudiante.ToDto();
    }

    public async Task UpdateAsync(
        int id, 
        UpdateEstudianteRequest dto, 
        CancellationToken cancellationToken = default)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var estudiante = await _unitOfWork.EstudianteRepository.GetEstudianteAsync
            (id, trackChanges: true, cancellationToken)
            ?? throw new NotFoundException($"Estudiante con id {id} no encontrado.");

        estudiante.ApplyUpdate(dto);
        _unitOfWork.EstudianteRepository.UpdateEstudiante(estudiante);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var estudiante = await _unitOfWork.EstudianteRepository.GetEstudianteAsync
            (id, trackChanges: true, cancellationToken)
            ?? throw new NotFoundException($"Estudiante con id {id} no encontrado.");

        _unitOfWork.EstudianteRepository.DeleteEstudiante(estudiante);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
