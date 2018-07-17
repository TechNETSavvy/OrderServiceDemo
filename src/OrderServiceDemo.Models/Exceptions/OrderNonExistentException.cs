using System;
using System.Collections.Generic;
using System.Text;

namespace OrderServiceDemo.Models.Exceptions
{
    public class OrderNonExistentException : Exception
    {
        public OrderNonExistentException(string message = null)
            : base(message)
        {

        }

        public OrderNonExistentException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
