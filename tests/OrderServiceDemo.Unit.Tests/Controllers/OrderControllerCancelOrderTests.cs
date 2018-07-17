using AutoMapper;
using NSubstitute;
using OrderServiceDemo.Controllers;
using OrderServiceDemo.Exceptions;
using OrderServiceDemo.Models.Exceptions;
using OrderServiceDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderServiceDemo.Unit.Tests.Controllers
{
    public class OrderControllerCancelOrderTests
    {
        private readonly ICancelOrderService _orderSerivce;
        private readonly IMapper _mapper;

        public OrderControllerCancelOrderTests()
        {
            var autoMapperConfig = Mapping.AutoMapperConfig.Configure();
            _mapper = autoMapperConfig.CreateMapper();

            _orderSerivce = Substitute.For<ICancelOrderService>();
        }

        [Fact]
        public async Task OrderController_WhenCancellingOrder_IfOrderAlreadyCancelled_ShouldReturn_HttpBadReqest()
        {
            //Arrange
            _orderSerivce
                .CancelOrder(Arg.Any<int>())
                .Returns(Task.FromException<Models.Order>(new OrderAlreadyCancelledException("The order sent for cancellation has already been cancelled.")));

            var controller = BuildController();

            //Act
            var result = await Assert.ThrowsAsync<StatusCodeException>(() => controller.CancelOrder(-100));

            //Assert
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task OrderController_WhenCancellingOrder_IfOrderDoesNotExist_ShouldReturn_HttpBadReqest()
        {
            //Arrange
            _orderSerivce
                .CancelOrder(Arg.Any<int>())
                .Returns(Task.FromException<Models.Order>(new OrderNonExistentException("The order sent for cancellation does not exist.")));

            var controller = BuildController();

            //Act
            var result = await Assert.ThrowsAsync<StatusCodeException>(() => controller.CancelOrder(-200));

            //Assert
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        private CancelOrderController BuildController() => new CancelOrderController(
            _mapper,
            _orderSerivce);
    }
}
