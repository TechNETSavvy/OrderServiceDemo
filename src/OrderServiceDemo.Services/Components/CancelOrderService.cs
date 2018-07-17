using OrderServiceDemo.Core;
using OrderServiceDemo.Models;
using OrderServiceDemo.Models.Exceptions;
using OrderServiceDemo.Services.Infrastructure;
using OrderServiceDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceDemo.Services.Components
{
    public class CancelOrderService : OrderService, ICancelOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderLineItemRepository _orderLineItemRepository;

        public CancelOrderService(
            IOrderRepository orderRepository,
            IOrderLineItemRepository orderLineItemRepository) :
                base (orderRepository, orderLineItemRepository)
        {
            _orderRepository = orderRepository;
            _orderLineItemRepository = orderLineItemRepository;
        }

        public IOrderLineItemRepository OrderLineItemRepository => _orderLineItemRepository;

        public async Task<Order> CancelOrder(int orderId)
        {
            var order = await _orderRepository.GetOrder(orderId);

            if (order == null)
                throw new OrderNonExistentException("Order sent to be cancelled does not exist.");

            if (order.OrderStatus == OrderStatus.Cancelled)
                throw new OrderAlreadyCancelledException("Order sent to be cancelled is already cancelled.");

            order.OrderStatus = OrderStatus.Cancelled;

            var cancelledOrder = await _orderRepository.UpdateOrder(order);

            return cancelledOrder;
        }
    }
}
