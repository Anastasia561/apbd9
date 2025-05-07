using apbd9.Model;
using apbd9.Model.Dto;

namespace apbd9.Services.Mappers;

public class CompletedOrderMapper
{
    public static CompletedOrder ToCompletedOrder(RequestDto requestDto, int orderId, float price)
    {
        return new CompletedOrder()
        {
            IdWarehouse = requestDto.IdWarehouse,
            IdProduct = requestDto.IdProduct,
            IdOrder = orderId,
            Amount = requestDto.Amount,
            Price = requestDto.Amount * price,
        };
    }
}