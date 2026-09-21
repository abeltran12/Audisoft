using Audisoft.Application.Contracts;
using Audisoft.Application.Contracts.Repositories;
using Audisoft.Application.Exceptions;
using Audisoft.Application.Requests;
using Audisoft.Application.Services;
using Audisoft.Domain;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace Audisoft.Application.UnitTests.Services;

public class EstudianteServiceTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly INotaRepository _notaRepository;
    private readonly IValidator<CreateEstudianteRequest> _createValidator;
    private readonly IValidator<UpdateEstudianteRequest> _updateValidator;
    private readonly EstudianteService _sut;

    public EstudianteServiceTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _estudianteRepository = Substitute.For<IEstudianteRepository>();
        _notaRepository = Substitute.For<INotaRepository>();
        _createValidator = Substitute.For<IValidator<CreateEstudianteRequest>>();
        _updateValidator = Substitute.For<IValidator<UpdateEstudianteRequest>>();

        _unitOfWork.EstudianteRepository.Returns(_estudianteRepository);
        _unitOfWork.NotaRepository.Returns(_notaRepository);

        _sut = new EstudianteService(_unitOfWork, _createValidator, _updateValidator);
    }

    [Fact]
    public async Task DeleteAsync_CuandoEstudianteNoExiste_DebeLanzarNotFoundException()
    {
        var estudianteId = 999;

        _estudianteRepository
            .GetEstudianteAsync(estudianteId, true, Arg.Any<CancellationToken>())
            .Returns((Estudiante?)null);

        var act = async () => await _sut.DeleteAsync(estudianteId);

        await act.Should().ThrowAsync<NotFoundException>();

        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
