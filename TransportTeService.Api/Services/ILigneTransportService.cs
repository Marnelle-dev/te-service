using TransportTeService.Api.DTOs;

namespace TransportTeService.Api.Services;

public interface ILigneTransportService
{
    Task<IEnumerable<LigneTransportResponse>> GetAllLignesTransportAsync();
    Task<LigneTransportResponse?> GetLigneTransportByIdAsync(Guid id);
    Task<LigneTransportResponse> CreateLigneTransportAsync(CreateLigneTransportRequest request, string? createdBy = null);
    Task<LigneTransportResponse?> UpdateLigneTransportAsync(Guid id, UpdateLigneTransportRequest request, string? modifiedBy = null);
    Task<bool> DeleteLigneTransportAsync(Guid id);
}

