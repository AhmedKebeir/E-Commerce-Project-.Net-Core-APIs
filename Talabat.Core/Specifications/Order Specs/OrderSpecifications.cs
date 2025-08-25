using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        public OrderSpecifications(string buyerEmail)
            : base(O => O.BuyerEmail == buyerEmail)
        {
            AddIncludes();

            AddOrderByDesc(O => O.OrderDate);
        }
        public OrderSpecifications(int id, string buyerEmail)
            : base(
                 O =>
                 (O.Id == id) &&
                 (O.BuyerEmail == buyerEmail)
                 )
        {
            AddIncludes();
        }

  

        private void AddIncludes()
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }

    }
}
