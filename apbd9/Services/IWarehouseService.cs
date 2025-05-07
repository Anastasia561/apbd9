namespace apbd9.Services;

public interface IWarehouseService
{
    public Task<bool> ValidateWarehouseAsync(int id, CancellationToken cancellationToken);
}