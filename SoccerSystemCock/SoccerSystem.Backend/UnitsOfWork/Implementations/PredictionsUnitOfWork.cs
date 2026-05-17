using SoccerSystem.Backend.Repositories.Interfaces;
using SoccerSystem.Backend.UnitsOfWork.Interfaces;
using SoccerSystem.Shared.DTOs;
using SoccerSystem.Shared.Entites;
using SoccerSystem.Shared.Responses;

namespace SoccerSystem.Backend.UnitsOfWork.Implementations;

public class PredictionsUnitOfWork : GenericUnitOfWork<Prediction>, IPredictionsUnitOfWork
{
    private readonly IPredictionsRepository _predictionsRepository;

    public PredictionsUnitOfWork(IGenericRepository<Prediction> repository, IPredictionsRepository predictionsRepository) : base(repository)
    {
        _predictionsRepository = predictionsRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Prediction>>> GetAsync(PaginationDTO pagination) => await _predictionsRepository.GetAsync(pagination);

    public override async Task<ActionResponse<Prediction>> GetAsync(int id) => await _predictionsRepository.GetAsync(id);

    public async Task<ActionResponse<Prediction>> AddAsync(PredictionDTO AddAsync) => await _predictionsRepository.AddAsync(AddAsync);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO paginationDTO) => await _predictionsRepository.GetTotalRecordsAsync(paginationDTO);

    public async Task<ActionResponse<Prediction>> UpdateAsync(PredictionDTO predictionDTO) => await _predictionsRepository.UpdateAsync(predictionDTO);
}