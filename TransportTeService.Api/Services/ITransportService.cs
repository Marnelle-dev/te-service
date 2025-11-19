using TransportTeService.Api.DTOs;

namespace TransportTeService.Api.Services;

public interface ITransportService
{
    Task<IEnumerable<TransportResponse>> GetAllTransportsAsync();
    Task<TransportResponse?> GetTransportByIdAsync(Guid id);
    Task<TransportResponse> CreateTransportAsync(CreateTransportRequest request, string? createdBy = null);
    Task<TransportResponse?> UpdateTransportAsync(Guid id, UpdateTransportRequest request, string? modifiedBy = null);
    Task<bool> DeleteTransportAsync(Guid id);
    Task<bool> SubmitTransportAsync(Guid id);
}

