using apbd9.Model;
using apbd9.Model.Dto;
using apbd9.Repositories;
using apbd9.Services.Mappers;

namespace apbd9.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> ValidateOrderAsync(int productId, int amount,
        CancellationToken cancellationToken)
    {
        var orderId = await _orderRepository.GetOrderIdByProductAndAmountAsync(productId, amount, cancellationToken);
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

    public async Task<int> FulfillOrderAsync(RequestDto requestDto, float price, CancellationToken cancellationToken)
    {
        var orderId =
            await _orderRepository.GetOrderIdByProductAndAmountAsync(requestDto.IdProduct, requestDto.Amount,
                cancellationToken);
        var completedOrder = CompletedOrderMapper.ToCompletedOrder(requestDto, orderId, price);
        return await _orderRepository.FulfillOrderAsync(completedOrder, cancellationToken);
    }
}