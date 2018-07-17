using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceDemo.Models.Exceptions
{
    public class OrderAlreadyCancelledException : Exception
    {
        public OrderAlreadyCancelledException(string message = null)
            : base(message)
        {
            
        }

        public OrderAlreadyCancelledException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
