namespace apbd9.Services;

public interface IOrderService
{
    public Task<bool> ValidateOrderAsync(int orderId, int productId, int amount, CancellationToken cancellationToken);
}