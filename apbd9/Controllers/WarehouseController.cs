using apbd9.Model.Dto;
using apbd9.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd9.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IOrderService orderService, IProductService productService,
        IWarehouseService warehouseService)
    {
        _orderService = orderService;
        _productService = productService;
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> FulfillOrderAsync(RequestDto requestDto, CancellationToken cancellationToken)
    {
        if (!await _productService.ValidateProductAsync(requestDto.IdProduct, cancellationToken) ||
            !await _warehouseService.ValidateWarehouseAsync(requestDto.IdWarehouse, cancellationToken) ||
            !await _orderService.ValidateOrderAsync(requestDto.IdProduct, requestDto.Amount, cancellationToken))

            return BadRequest();

        var price = await _productService.GetProductPriceByIdAsync(requestDto.IdProduct, cancellationToken);
        var result = await _orderService.FulfillOrderAsync(requestDto, price, cancellationToken);
        return Ok(result);
    }
}