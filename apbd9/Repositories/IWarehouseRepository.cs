namespace apbd9.Repositories;

public interface IWarehouseRepository
{
    public Task<bool> CheckIfWarehouseExistsAsync(int id, CancellationToken cancellationToken);
}