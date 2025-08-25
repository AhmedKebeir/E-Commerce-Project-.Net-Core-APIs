using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class AppicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            //builder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>();
            //builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
            //builder.Services.AddScoped<IGenericRepository<ProductCategory>, GenericRepository<ProductCategory>>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            
            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            services.AddAutoMapper(typeof(MappingProfiles));


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                    .SelectMany(P => P.Value.Errors).Select(E => E.ErrorMessage)
                    .ToArray();
                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            return services;
        }
    }
}
