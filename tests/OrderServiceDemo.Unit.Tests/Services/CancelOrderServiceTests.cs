using NSubstitute;
using OrderServiceDemo.Models.Exceptions;
using OrderServiceDemo.Services.Components;
using OrderServiceDemo.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderServiceDemo.Unit.Tests.Services
{
    public class CancelOrderServiceTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderLineItemRepository _orderLineItemRepository;

        public CancelOrderServiceTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _orderLineItemRepository = Substitute.For<IOrderLineItemRepository>();
        }

        public IOrderLineItemRepository OrderLineItemRepository => _orderLineItemRepository;

        [Fact]
        public async Task OrderService_WhenCancellingOrder_IfOrderAlreadyCancelled_ThrowsOrderAlreadyCancelledException()
        {
            //Arrange
            var orderId = 1;
            var service = BuildService();

            //Act && Assert
            var result = await Assert.ThrowsAsync<OrderAlreadyCancelledException>(() => service.CancelOrder(orderId));
        }

        [Fact]
        public async Task OrderService_WhenCancellingOrder_IfOrderDoesNotExist_ThrowsOrderNonExistentException()
        {
            //Arrange
            var orderId = -100;
            var service = BuildService();

            //Act && Assert
            var result = await Assert.ThrowsAsync<OrderNonExistentException>(() => service.CancelOrder(orderId));
        }

        private CancelOrderService BuildService() => new CancelOrderService(
            _orderRepository,
            OrderLineItemRepository);
    }
}
