﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Entities.ViewModels
{
    public class OrderVM
    {
        public OrderProduct OrderProduct { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }

    }
}
