namespace apbd9.Repositories;

public interface IProductRepository
{
    public Task<bool> CheckIfProductExistsAsync(int id, CancellationToken cancellationToken);
    public Task<float> GetProductPriceByIdAsync(int id, CancellationToken cancellationToken);
}