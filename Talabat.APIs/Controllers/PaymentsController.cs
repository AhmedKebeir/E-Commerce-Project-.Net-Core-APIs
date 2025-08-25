using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string endpointSecret = "";

        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400, "An Error with your basket!"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["stripe:Signatur"], endpointSecret);

            var paymntIntent = (PaymentIntent)stripeEvent.Data.Object;

            Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymntIntent.Id, true);
                    _logger.LogInformation("Payment is succeeded ya Hamada ", paymntIntent.Id);
                    break;
                case "payment_intent.payment_failed":
                    order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymntIntent.Id, false);
                    _logger.LogInformation("Payment is Failed ya Hamada ", paymntIntent.Id);
                    break;

            }

            return Ok();

        }
    }
}
