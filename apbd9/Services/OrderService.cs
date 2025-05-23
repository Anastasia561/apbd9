﻿using apbd9.Model.Dto;
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
        var exists = await _orderRepository.CheckIfOrderExistsAsync(productId, amount, cancellationToken);
        if (!exists) throw new ArgumentException("Order not found");
        var orderId =
            await _orderRepository.GetOrderIdByProductAndAmountAsync(productId, amount, cancellationToken);
        var completed = await _orderRepository.CheckIfOrderCompletedAsync(orderId, cancellationToken);
        if (!completed)
            return true;
        throw new Exception($"Order with id - {orderId} is already completed");
    }

    public async Task<int> FulfillOrderAsync(RequestDto requestDto, float price, CancellationToken cancellationToken)
    {
        var orderId =
            await _orderRepository.GetOrderIdByProductAndAmountAsync(requestDto.IdProduct, requestDto.Amount,
                cancellationToken);
        var completedOrder = CompletedOrderMapper.ToCompletedOrder(requestDto, orderId, price);
        return await _orderRepository.FulfillOrderAsync(completedOrder, cancellationToken);
    }

    public async Task<int> FulfillOrderUsingProcedureAsync(RequestDto requestDto,
        CancellationToken cancellationToken)
    {
        return await _orderRepository.FulfillOrderUsingProcedureAsync(requestDto.IdProduct, requestDto.IdWarehouse,
            requestDto.Amount, requestDto.CreatedAt, cancellationToken);
    }
}