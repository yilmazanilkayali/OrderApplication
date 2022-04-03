using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Data.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private ApplicationDbContext _contex;   
        public CartRepository(ApplicationDbContext contex) : base(contex)
        {
            _contex = contex;
        }

        public int IncreaseCount(Cart cart, int count)
        {
            cart.Count += count;
            return cart.Count;
        }
    }
}
