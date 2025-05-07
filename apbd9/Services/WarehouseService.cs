using apbd9.Repositories;

namespace apbd9.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<bool> ValidateWarehouseAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _warehouseRepository.CheckIfWarehouseExistsAsync(id, cancellationToken);
        if (!result)
            throw new ArgumentException($"Warehouse with id - {id} not found");
        return result;
    }
}