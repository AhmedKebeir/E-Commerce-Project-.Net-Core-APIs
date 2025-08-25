using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        
        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto model)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<AddressDto,Address>(model.shipToAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, model.BasketId, model.DeliveryMethodId, address);
            
            if (order is null) return BadRequest(new ApiResponse(400));

            var mapping = _mapper.Map<Order, OrderToReturnDto>(order);

            return Ok(mapping);

        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            var mapping = _mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(orders);

            return Ok(mapping);
        }
        [ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(id,buyerEmail);

            if (order is null) return NotFound(new ApiResponse(404));

            var mapping = _mapper.Map<Order, OrderToReturnDto>(order);

            return Ok(mapping);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var deliveryMethods = await _orderService.GetGeliveryMethodsAsync();

            return Ok(deliveryMethods);
        }
    }
}
