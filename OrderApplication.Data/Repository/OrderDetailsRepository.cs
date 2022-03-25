using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetail>, IOrderDetailsRepository
    {
        private ApplicationDbContext _contex;
        public OrderDetailsRepository(ApplicationDbContext contex) : base(contex)
        {
            _contex = contex;
        }
    }
}
