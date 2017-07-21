using PhoneStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Data.Abstarct
{
    public interface IOrderProcessor
    {
        void ProcessOrder(ShoppingCart cart, OrderDetails orderDetails);
    }
}
