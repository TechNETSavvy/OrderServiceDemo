using AutoMapper;
using NSubstitute;
using OrderServiceDemo.Controllers;
using OrderServiceDemo.Exceptions;
using OrderServiceDemo.Models.Exceptions;
using OrderServiceDemo.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OrderServiceDemo.Unit.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly ICancelOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderControllerTests()
        {
            var autoMapperConfig = Mapping.AutoMapperConfig.Configure();
            _mapper = autoMapperConfig.CreateMapper();

            _orderService = Substitute.For<ICancelOrderService>();
        }

        [Fact]
        public async Task OrderController_WhenCreatingOrder_IfInvalidRequest_ShouldReturn_HttpBadReqest()
        {
            //Arrange
            _orderService
                .CreateOrder(Arg.Any<Models.Order>())
                .Returns(Task.FromException<Models.Order>(new InvalidRequestException("The Message")));

            var controller = BuildController();

            //Act
            var result = await Assert.ThrowsAsync<StatusCodeException>(() => controller.CreateOrder(new Core.v1.RequestModels.CreateOrderRequest
            {
                OrderLineItems = new List<Core.v1.OrderLineItem>()
            }));

            //Assert
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task OrderController_WhenCancellingOrder_IfOrderAlreadyCancelled_ShouldReturn_HttpBadReqest()
        {
            //Arrange
            _orderService
                .CancelOrder(Arg.Any<int>())
                .Returns(Task.FromException<Models.Order>(new OrderAlreadyCancelledException("The order sent for cancellation has already been cancelled.")));

            var controller = BuildController();

            //Act
            var result = await Assert.ThrowsAsync<StatusCodeException>(() => controller.CancelOrder(1));

            //Assert
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task OrderController_WhenCancellingOrder_IfOrderDoesNotExist_ShouldReturn_HttpBadReqest()
        {
            //Arrange
            _orderService
                .CancelOrder(Arg.Any<int>())
                .Returns(Task.FromException<Models.Order>(new OrderNonExistentException("The order sent for cancellation does not exist.")));

            var controller = BuildController();

            //Act
            var result = await Assert.ThrowsAsync<StatusCodeException>(() => controller.CancelOrder(-200));

            //Assert
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task OrderController_WhenDeletingOrder_IfOrderDoesNotExist_ShouldReturn_HttpBadReqest()
        {
            //Arrange
            _orderService
                .DeleteOrder(Arg.Any<int>())
                .Returns(Task.FromException<Models.Order>(new OrderNonExistentException("The order sent for deletion does not exist.")));

            var controller = BuildController();

            //Act
            var result = await Assert.ThrowsAsync<StatusCodeException>(() => controller.DeleteOrder(-200));

            //Assert
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        private OrderController BuildController() => new OrderController(
            _mapper, 
            _orderService);
    }
}
