using apbd9.Model;

namespace apbd9.Repositories;

public interface IOrderRepository
{
    public Task<bool> CheckIfOrderExistsAsync(int productId, int amount, CancellationToken cancellationToken);
    public Task<bool> CheckIfOrderCompletedAsync(int id, CancellationToken cancellationToken);
    public Task<int> GetOrderIdByProductAndAmountAsync(int productId, int amount, CancellationToken cancellationToken);
    public Task<int> FulfillOrderAsync(CompletedOrder completedOrder, CancellationToken cancellationToken);
}