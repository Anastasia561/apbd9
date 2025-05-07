using apbd9.Repositories;

namespace apbd9.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IConfiguration configuration, IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> ValidateProductAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _productRepository.CheckIfProductExistsAsync(id, cancellationToken);
        if (!result)
            throw new ArgumentException($"Product with id - {id} not found");

        return result;
    }

    public async Task<float> GetProductPriceByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _productRepository.GetProductPriceByIdAsync(id, cancellationToken);
    }
}