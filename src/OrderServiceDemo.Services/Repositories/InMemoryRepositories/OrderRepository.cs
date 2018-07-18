using OrderServiceDemo.Models;
using OrderServiceDemo.Models.Exceptions;
using OrderServiceDemo.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceDemo.Services.Repositories.InMemoryRepositories
{
    public class OrderRepository : InMemoryRepository<Order>, IOrderRepository
    {
        private readonly IOrderLineItemRepository _orderLineItemRepository;

        public OrderRepository(IOrderLineItemRepository orderLineItemRepository)
        {
            _orderLineItemRepository = orderLineItemRepository;
        }

        //The change to this implementation is because of the bug found below.
        protected override Action<Order> SetIdentity => ((x) =>
        {
            if(x.OrderId == 0)
                x.OrderId = Entities.Count() + 1;
        });

        public Task<Order> CreateOrder(Order order)
        {
            return AddEntity(order);
        }

        public async Task<Order> DeleteOrder(Order order)
        {
            var lineItems = await _orderLineItemRepository.GetOrderLineItems(order.OrderId);
            if (lineItems?.Any() == true)
                throw new InMemoryRepositoryException($"Simulated Foreign Key Constraint - Line Items exist for order {order.OrderId}");

            return await DeleteEntity(order);
        }

        public Task<Order> GetOrder(int orderId)
        {
            var order = Entities.SingleOrDefault(x => x.OrderId == orderId);
            return Task.FromResult(order);
        }

        public Task<IEnumerable<Order>> GetOrders(int userId)
        {
            var orders = Entities.Where(x => x.UserId == userId);
            return Task.FromResult(orders);
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            var existing = Entities.SingleOrDefault(x => x.OrderId == order.OrderId);
            if (existing == null)
                return null; //This would most closely match the behavior of an update sql script (with output inserted.*) that updated nothing.

            //Noticed a small bug here where any time an order was deleted then re-added as an update the "identity"
            //would increment again causing multiple orders with the same OrderId to exist causing the SingleOrDefault
            //method call in GetOrder to throw an exception.
            await DeleteEntity(existing);

            var result = await AddEntity(order);
            return result;
        }
    }
}
