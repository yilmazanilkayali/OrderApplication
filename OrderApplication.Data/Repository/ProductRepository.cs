using OrderApplication.Data.Repository.IRepository;
using OrderApplication.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Data.Repository
{
   
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _contex;
        public ProductRepository(ApplicationDbContext contex) : base(contex)
        {
            _contex = contex;
        }
    }
}
