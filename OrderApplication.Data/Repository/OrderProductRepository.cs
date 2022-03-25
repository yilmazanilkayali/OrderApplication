using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Data.Repository
{
    public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
    {
        private ApplicationDbContext _contex;
        public OrderProductRepository(ApplicationDbContext contex) : base(contex)
        {
            _contex = contex;
        }
    }
}
