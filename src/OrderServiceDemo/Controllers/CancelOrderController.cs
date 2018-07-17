using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderServiceDemo.Core.v1;
using OrderServiceDemo.Models.Exceptions;
using OrderServiceDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderServiceDemo.Controllers
{
    public class CancelOrderController : OrderController
    {
        private readonly ICancelOrderService _orderService;
        private readonly IMapper _mapper;

        public CancelOrderController(
            IMapper mapper,
            ICancelOrderService orderService) :
                base(mapper, orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        [Route("v1/orders/{orderId:int}/cancel")]
        public async Task<Order> CancelOrder(int orderId)
        {
            //TODO: Add controller implementation.
            try
            {
                var cancelledOrder = await _orderService.CancelOrder(orderId);
                var response = _mapper.Map<Order>(cancelledOrder);
                return response;
            }
            catch(Exception ex)
            {
                if(ex is OrderAlreadyCancelledException)
                {

                }
                else if(ex is OrderNonExistentException)
                {

                }

                throw BuildExceptionResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
