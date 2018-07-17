using NSubstitute;
using OrderServiceDemo.Core;
using OrderServiceDemo.Models.Exceptions;
using OrderServiceDemo.Services.Components;
using OrderServiceDemo.Services.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OrderServiceDemo.Unit.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderLineItemRepository _orderLineItemRepository;

        public OrderServiceTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _orderLineItemRepository = Substitute.For<IOrderLineItemRepository>();
        }

        [Fact]
        public async Task OrderService_WhenCreatingOrder_IfNoLineItems_ThrowsInvalidRequestException()
        {
            //Arrange
            var order = new Models.Order();
            var service = BuildService();

            //Act && Assert
            var result = await Assert.ThrowsAsync<InvalidRequestException>(() => service.CreateOrder(order));
        }

        [Fact]
        public async Task OrderService_WhenCancellingOrder_IfOrderAlreadyCancelled_ThrowsOrderAlreadyCancelledException()
        {
            //Arrange
            var service = BuildService();

            //Act && Assert
            var result = await Assert.ThrowsAsync<OrderAlreadyCancelledException>(() => service.CancelOrder(1));
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

        [Fact]
        public async Task OrderService_WhenDeletingOrder_IfOrderDoesNotExist_ThrowsOrderNonExistentException()
        {
            //Arrange
            var orderId = -100;
            var service = BuildService();

            //Act && Assert
            var result = await Assert.ThrowsAsync<OrderNonExistentException>(() => service.DeleteOrder(orderId));
        }

        private OrderService BuildService() => new OrderService(
            _orderRepository,
            _orderLineItemRepository);
    }
}
