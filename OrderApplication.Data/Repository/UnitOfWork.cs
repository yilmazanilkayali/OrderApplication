using OrderApplication.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        //UOW:Tüm instance işlemlerinin tek bir yerden tek instance oluşturularak kullanılmasına olanak sağlar.

        private ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        public IAppUserRepository AppUser => new AppUserRepository(_context);

        public ICartRepository Cart => new CartRepository(_context);

        public ICategoryRepository Category => new CategoryRepository(_context);

        public IOrderDetailsRepository OrderDetails => new OrderDetailsRepository(_context);

        public IOrderProductRepository OrderProduct => new OrderProductRepository(_context);

        public IProductRepository Product => new ProductRepository(_context);

     

        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            //save işlemi bittikten sonra işlemin ramde gerksiz tutulmasını önlemek amacıylaa kullanılır. tamamlanan işlevi ramden kaldırır. performansa yöneliktir.
            _context.Dispose();
        }
    }
}
