using OrderServiceDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceDemo.Services.Interfaces
{
    public interface ICancelOrderService : IOrderService
    {
        /// <summary>
        /// Cancels the order supplied. Throws <see cref="Models.Exceptions.InvalidRequestException"/>
        /// when an order id is supplied for an order that has already been <see cref="Core.OrderStatus.Cancelled"/>
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>The deleted Order</returns>
        Task<Order> CancelOrder(int orderId);
    }
}
