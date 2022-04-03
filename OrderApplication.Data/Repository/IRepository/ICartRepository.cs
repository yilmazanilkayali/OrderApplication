using OrderApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Data.Repository.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        int IncreaseCount(Cart cart, int count);
    }
}
