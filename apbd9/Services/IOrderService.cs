using apbd9.Model;
using apbd9.Model.Dto;

namespace apbd9.Services;

public interface IOrderService
{
    public Task<bool> ValidateOrderAsync(int orderId, int productId, int amount, CancellationToken cancellationToken);
    public Task<int> FulfillOrderAsync(RequestDto requestDto,int price, CancellationToken cancellationToken);
}