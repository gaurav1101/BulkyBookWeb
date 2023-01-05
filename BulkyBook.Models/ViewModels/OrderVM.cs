﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class OrderVM
    {
        public OrderHeader orderHeader { get; set; }
        public IEnumerable<OrderDetail> orderDetails { get; set; }
    }
}
