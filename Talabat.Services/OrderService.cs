using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            
            ///IGenericRepository<Product> productRepo,
            ///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            ///IGenericRepository<Order> orderRepo

            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get Basket For Baskets Repo
            var basket = await _basketRepository.GetBasketAsync(basketId);

            //2. Get Selected Items at Basket From Repo
            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                var productRepository = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepository.GetAsync(item.Id);

                    var productItemOrder = new ProductItemOrder(item.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate SubTotal
            var subtotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

            // 4. Get DeliveryMethod From DeliveryMethods Repo 
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);


            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existingOrder = await orderRepo.GetSpecAsync(spec);

            if(existingOrder != null)
            {
                orderRepo.Delete(existingOrder);

                await _paymentService.CreateUpdatePaymentIntent(basketId);
            }


            // 5. Create Order

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal, basket.PaymentIntentId);

            await orderRepo.AddAsync(order);

            // 6. Save To Database [TODO]
            var result = await _unitOfWork.CompleteAsync();


            if (result <= 0) return null;

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail);

            var orders = await ordersRepo.GetAllSpecAsync(spec);

            return orders;
        }
        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderSpec = new OrderSpecifications(orderId, buyerEmail);
            var order = _unitOfWork.Repository<Order>().GetSpecAsync(orderSpec);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetGeliveryMethodsAsync()
        {
            var deliveryMethodsRepo = _unitOfWork.Repository<DeliveryMethod>();

            var deliveryMethods = await deliveryMethodsRepo.GetAllAsync();

            return deliveryMethods;
        }
    }
}
