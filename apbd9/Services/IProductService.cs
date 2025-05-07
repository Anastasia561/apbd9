namespace apbd9.Services;

public interface IProductService
{
    public Task<bool> ValidateProductAsync(int id, CancellationToken cancellationToken);
    public Task<float> GetProductPriceByIdAsync(int id, CancellationToken cancellationToken);
}