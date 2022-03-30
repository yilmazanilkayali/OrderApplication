using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Entities.ViewModels
{
    public class CartVM
    {
        public OrderProduct OrderProduct { get; set; }
        public IEnumerable<Cart> CartList { get; set; }

    }
}
