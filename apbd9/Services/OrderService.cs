using apbd9.Repositories;

namespace apbd9.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> ValidateOrderAsync(int orderId, int productId, int amount,
        CancellationToken cancellationToken)
    {
        var exists = await _orderRepository.CheckIfOrderExistsAsync(productId, amount, cancellationToken);
        var completed = await _orderRepository.CheckIfOrderCompletedAsync(orderId, cancellationToken);

        switch (exists)
        {
            case true when !completed:
                return true;
            case false:
                throw new ArgumentException($"Order with id - {orderId} not found");
        }

        if (completed)
            throw new Exception($"Order with id - {orderId} is already completed");

        return false;
    }
}